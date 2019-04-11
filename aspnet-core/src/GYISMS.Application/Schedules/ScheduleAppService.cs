
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

using GYISMS.Schedules.Authorization;
using GYISMS.Schedules.Dtos;
using GYISMS.Schedules;
using GYISMS.Authorization;
using GYISMS.GYEnums;
using GYISMS.Dtos;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.ScheduleDetails;
using Abp.Auditing;
using GYISMS.Helpers;
using GYISMS.DingDing;
using GYISMS.DingDing.Dtos;
using GYISMS.SystemDatas;
using GYISMS.Authorization.Users;
using GYISMS.Organizations.Dtos;
using Senparc.CO2NET.Helpers;
using System.IO;
using System.Text;
using Senparc.CO2NET.HttpUtility;
using GYISMS.GrowerAreaRecords;
using GYISMS.GrowerLocationLogs;
using GYISMS.VisitRecords;
using GYISMS.Growers;
using GYISMS.VisitTasks;
using GYISMS.ScheduleTasks;
using GYISMS.ScheduleTasks.Dtos;
using GYISMS.VisitExamines;
using GYISMS.TaskExamines;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace GYISMS.Schedules
{
    /// <summary>
    /// Schedule应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]

    public class ScheduleAppService : GYISMSAppServiceBase, IScheduleAppService
    {
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly IScheduleManager _scheduleManager;
        private readonly ISheduleDetailRepository _scheduledetailRepository;
        private readonly IDingDingAppService _dingDingAppService;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly IRepository<User, long> _userRepository;
        //private string accessToken;
        //private DingDingAppConfig ddConfig;
        private readonly IRepository<ScheduleTask, Guid> _scheduletaskRepository;
        private readonly IRepository<ScheduleDetail, Guid> _scheduleDetailRepository;
        private readonly IRepository<VisitTask> _visitTaskRepository;
        private readonly IRepository<Grower> _growerRepository;
        private readonly IRepository<VisitRecord, Guid> _visitRecordRepository;
        private readonly IRepository<GrowerLocationLog, Guid> _growerLocationLogRepository;
        private readonly IRepository<GrowerAreaRecord, Guid> _growerAreaRecordRepository;
        private readonly IRepository<VisitExamine, Guid> _visitexamineRepository;
        private readonly IRepository<TaskExamine, int> _taskexamineRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleAppService(IRepository<Schedule, Guid> scheduleRepository
            , IScheduleManager scheduleManager
            , ISheduleDetailRepository scheduledetailRepository
            , IDingDingAppService dingDingAppService
            , IRepository<SystemData, int> systemdataRepository
            , IRepository<User, long> userRepository

            , IRepository<ScheduleTask, Guid> scheduletaskRepository
            , IScheduleTaskManager scheduletaskManager
            , IRepository<VisitTask> visitTaskRepository
            , IRepository<ScheduleDetail, Guid> scheduleDetailRepository
            , IRepository<Grower> growerRepository
            , IRepository<VisitRecord, Guid> visitRecordRepository
            , IRepository<GrowerLocationLog, Guid> growerLocationLogRepository
            , IRepository<GrowerAreaRecord, Guid> growerAreaRecordRepository
            , IRepository<VisitExamine, Guid> visitexamineRepository
            , IRepository<TaskExamine, int> taskexamineRepository
            , IHostingEnvironment hostingEnvironment
            )
        {
            _scheduleRepository = scheduleRepository;
            _scheduleManager = scheduleManager;
            _scheduledetailRepository = scheduledetailRepository;
            _dingDingAppService = dingDingAppService;
            _userRepository = userRepository;

            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
            _visitRecordRepository = visitRecordRepository;
            _systemdataRepository = systemdataRepository;
            _growerLocationLogRepository = growerLocationLogRepository;
            _growerAreaRecordRepository = growerAreaRecordRepository;
            _visitexamineRepository = visitexamineRepository;
            _taskexamineRepository = taskexamineRepository;
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// 获取Schedule的分页列表信息
        ///</summary>
        public async Task<PagedResultDto<ScheduleListDto>> GetPagedSchedulesAsync(GetSchedulesInput input)
        {
            var isAdmin = await CheckAdminAsync();

            var query = _scheduleRepository.GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                        .WhereIf(input.ScheduleType.HasValue, r => r.Type == input.ScheduleType)
                        .WhereIf(!isAdmin, s => s.Status == ScheduleMasterStatusEnum.已发布 || (s.Status == ScheduleMasterStatusEnum.草稿 && s.CreatorUserId == AbpSession.UserId));

            var user = _userRepository.GetAll();
            var entity = from q in query
                         join u in user on q.CreatorUserId equals u.Id
                         //into table
                         //from t in table.DefaultIfEmpty()
                         select new ScheduleListDto()
                         {
                             Id = q.Id,
                             Name = q.Name,
                             Type = q.Type,
                             Status = q.Status,
                             PublishTime = q.PublishTime,
                             CreateUserName = u.Name,
                             //Area = t.Area != null ? t.Area : " - ",
                             BeginTime = q.BeginTime,
                             EndTime = q.EndTime
                         };


            var scheduleCount = query.Count();

            var schedules = entity
                    .OrderBy(v => v.Status).ThenByDescending(v => v.PublishTime).AsNoTracking()
                    .PageBy(input)
                    .ToList();
            var ids = schedules.Select(s => s.Id).ToArray();

            var detail = _scheduledetailRepository.GetAll();
            var percentageQuery = (from d in detail
                                   where ids.Contains(d.ScheduleId)
                                   group new
                                   {
                                       d.ScheduleId,
                                       d.VisitNum,
                                       d.CompleteNum
                                   } by new
                                   {
                                       d.ScheduleId
                                   } into g
                                   select new
                                   {
                                       Id = g.Key.ScheduleId,
                                       CompleteCount = g.Sum(v => v.CompleteNum),
                                       VisitCount = g.Sum(v => v.VisitNum),
                                   });
            var percentageList = percentageQuery.ToList();

            foreach (var item in schedules)
            {
                var percentage = percentageList.Where(p => p.Id == item.Id).FirstOrDefault();
                if (percentage != null)
                {
                    item.VisitCount = percentage.VisitCount;
                    item.CompleteCount = percentage.CompleteCount;
                }
            }
            return new PagedResultDto<ScheduleListDto>(
                    scheduleCount,
                    schedules
                );
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetScheduleForEditOutput> GetScheduleForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetScheduleForEditOutput();
            ScheduleEditDto scheduleEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _scheduleRepository.GetAsync(input.Id.Value);

                scheduleEditDto = entity.MapTo<ScheduleEditDto>();

                //scheduleEditDto = ObjectMapper.Map<List <scheduleEditDto>>(entity);
            }
            else
            {
                scheduleEditDto = new ScheduleEditDto();
            }

            output.Schedule = scheduleEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Schedule的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateSchedule(CreateOrUpdateScheduleInput input)
        {

            if (input.Schedule.Id.HasValue)
            {
                await UpdateScheduleAsync(input.Schedule);
            }
            else
            {
                await CreateScheduleAsync(input.Schedule);
            }
        }


        /// <summary>
        /// 新增Schedule
        /// </summary>
        protected virtual async Task<ScheduleEditDto> CreateScheduleAsync(ScheduleEditDto input)
        {
            var entity = input.MapTo<Schedule>(); //ObjectMapper.Map<Schedule>(input);
            await _scheduleRepository.InsertAsync(entity);
            return entity.MapTo<ScheduleEditDto>();
        }

        /// <summary>
        /// 编辑Schedule
        /// </summary>
        protected virtual async Task<ScheduleEditDto> UpdateScheduleAsync(ScheduleEditDto input)
        {
            var entity = await _scheduleRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            var result = await _scheduleRepository.UpdateAsync(entity);
            return result.MapTo<ScheduleEditDto>();
        }



        /// <summary>
        /// 删除Schedule信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ScheduleAppPermissions.Schedule_Delete)]
        public async Task DeleteSchedule(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _scheduleRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Schedule的方法
        /// </summary>
        [AbpAuthorize(ScheduleAppPermissions.Schedule_BatchDelete)]
        public async Task BatchDeleteSchedulesAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _scheduleRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 新增或修改计划信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ScheduleEditDto> CreateOrUpdateScheduleAsycn(ScheduleEditDto input)
        {
            if (input.Status == ScheduleMasterStatusEnum.已发布)
            {
                input.PublishTime = DateTime.Now;
            }
            if (input.Id.HasValue)
            {
                return await UpdateScheduleAsync(input);
            }
            else
            {
                return await CreateScheduleAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ScheduleListDto> GetScheduleByIdAsync(Guid id)
        {
            var entity = await _scheduleRepository.GetAsync(id);
            return entity.MapTo<ScheduleListDto>();
        }

        /// <summary>
        /// 删除计划信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ScheduleDeleteByIdAsync(ScheduleEditDto input)
        {
            await _scheduleRepository.DeleteAsync(input.Id.Value);
            //var entity = await _scheduleRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);
            //entity.IsDeleted = true;
            //entity.DeletionTime = DateTime.Now;
            //entity.DeleterUserId = AbpSession.UserId;
            //await _scheduleRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 获取AccessToken ToDo钉钉配置
        /// </summary>
        /// <returns></returns>
        //private string GetAccessToken()
        //{
        //    DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
        //    OapiGettokenRequest request = new OapiGettokenRequest();
        //    request.Appkey = "ding7xespi5yumrzraaq";
        //    request.Appsecret = "idKPu4wVaZjBKo6oUvxcwSQB7tExjEbPaBpVpCEOGlcZPsH4BDx-sKilG726-nC3";
        //    request.SetHttpMethod("GET");
        //    OapiGettokenResponse response = client.Execute(request);
        //    return response.AccessToken;
        //}

        /// <summary>
        /// 上传图片并返回MeadiaId
        /// </summary>
        public object UpdateAndGetMediaId(string path)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/media/upload");
            OapiMediaUploadRequest request = new OapiMediaUploadRequest();
            request.Type = "image";
            request.Media = new Top.Api.Util.FileItem($@"{path}");
            DingDingAppConfig ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.任务拜访);
            string accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
            OapiMediaUploadResponse response = client.Execute(request, accessToken);
            return response;
        }


        /// <summary>
        /// 发送钉钉工作通知
        /// </summary>
        public async Task<APIResultDto> SendMessageToEmployeeAsync(GetSchedulesInput input)
        {
            try
            {
                //获取消息模板配置
                string messageTitle = await _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.烟叶服务 && v.Type == ConfigType.烟叶公共 && v.Code == GYCode.MessageTitle).Select(v => v.Desc).FirstOrDefaultAsync();
                string messageMediaId = await _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.烟叶服务 && v.Type == ConfigType.烟叶公共 && v.Code == GYCode.MediaId).Select(v => v.Desc).FirstOrDefaultAsync();
                //获取UserIds
                int pageIndex = 1; //skip
                int pageSize = 20; //take
                int count = await _scheduledetailRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId).Select(v => v.EmployeeId).Distinct().AsNoTracking().CountAsync();
                var ids = await _scheduledetailRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId).Select(v => v.EmployeeId).Distinct().AsNoTracking().ToListAsync();
                float frequency = (float)count / pageSize;//计算次数
                DingDingAppConfig ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.任务拜访);
                string accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
                for (int i = 0; i < Math.Ceiling(frequency); i++)
                {
                    var temp = ids.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    string tempIds = string.Join(",", temp.ToArray());
                    //发送工作消息
                    /*IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");
                    OapiMessageCorpconversationAsyncsendV2Request request = new OapiMessageCorpconversationAsyncsendV2Request();
                    request.UseridList = tempIds;
                    request.ToAllUser = false;
                    request.AgentId = ddConfig.AgentID;

                    OapiMessageCorpconversationAsyncsendV2Request.MsgDomain msg = new OapiMessageCorpconversationAsyncsendV2Request.MsgDomain();
                    msg.Link = new OapiMessageCorpconversationAsyncsendV2Request.LinkDomain();
                    msg.Msgtype = "link";
                    msg.Link.Title = messageTitle;
                    msg.Link.Text = input.ScheduleName + " " + DateTime.Now.ToString();
                    msg.Link.PicUrl = messageMediaId;
                    msg.Link.MessageUrl = "eapp://";
                    request.Msg_ = msg;
                    OapiMessageCorpconversationAsyncsendV2Response response = client.Execute(request, accessToken);*/
                    var msgdto = new DingMsgDto();
                    msgdto.userid_list = tempIds;
                    msgdto.to_all_user = false;
                    msgdto.agent_id = ddConfig.AgentID;
                    msgdto.msg.msgtype = "link";
                    msgdto.msg.link.title = messageTitle;
                    msgdto.msg.link.text = input.ScheduleName + " " + DateTime.Now.ToString();
                    msgdto.msg.link.picUrl = messageMediaId;
                    msgdto.msg.link.messageUrl = "eapp://";
                    var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
                    var jsonString = SerializerHelper.GetJsonString(msgdto, null);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        var obj = Post.PostGetJson<object>(url, null, ms);
                    };
                    pageIndex++;
                }
                return new APIResultDto() { Code = 0, Msg = "钉钉消息发送成功" };
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SendMessageToEmployeeAsync errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "钉钉消息发送失败" };
            }
        }

        /// <summary>
        /// APP获取需要同步的数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<AppSyncData> GetSyncDataAppTask(string userId)
        {
            AppSyncData result = new AppSyncData();
            var query = from st in _scheduletaskRepository.GetAll()
                        join sd in _scheduleDetailRepository.GetAll() on st.Id equals sd.ScheduleTaskId
                        join t in _visitTaskRepository.GetAll() on st.TaskId equals t.Id
                        join s in _scheduleRepository.GetAll() on st.ScheduleId equals s.Id
                        where sd.EmployeeId == userId
                        && (sd.Status == ScheduleStatusEnum.未开始 || sd.Status == ScheduleStatusEnum.进行中)
                        && s.Status == ScheduleMasterStatusEnum.已发布
                        && s.EndTime >= DateTime.Today
                        select new
                        {
                            ScheduleId = s.Id,
                            ScheduleDetailId = sd.Id,
                            ScheduleTaskId = st.Id,
                            VistTaskId = t.Id,
                        };
            var sIds = await query.Select(v => v.ScheduleId).Distinct().ToListAsync();
            var sdIds = await query.Select(v => v.ScheduleDetailId).Distinct().ToListAsync();
            var stIds = await query.Select(v => v.ScheduleTaskId).Distinct().ToListAsync();
            var vIds = query.Select(v => v.VistTaskId).Distinct();
            foreach (var item in sIds) //同步计划
            {
                result.ScheduleList.Add(await _scheduleRepository.GetAll().Where(v => v.Id == item).AsNoTracking().FirstOrDefaultAsync());
            }
            foreach (var item in sdIds) //同步计划详情and拜访记录and拜访考核and面积落实记录
            {
                var entity = await _scheduleDetailRepository.GetAll().Where(v => v.Id == item).AsNoTracking().FirstOrDefaultAsync();
                result.ScheduleDetailList.Add(entity);
                await CurrentUnitOfWork.SaveChangesAsync();
                var vistRecord = await _visitRecordRepository.GetAll().Where(v => v.ScheduleDetailId == item).AsNoTracking().ToListAsync();
                await CurrentUnitOfWork.SaveChangesAsync();
                result.VisitRecordList.AddRange(vistRecord);
                foreach (var vr in vistRecord)
                {
                    var vistExamine = await _visitexamineRepository.GetAll().Where(v => v.VisitRecordId == vr.Id).AsNoTracking().ToListAsync();
                    result.VisitExamineList.AddRange(vistExamine);
                }
                var growerAreaRecord = await _growerAreaRecordRepository.GetAll().Where(v => v.ScheduleDetailId == item).AsNoTracking().ToListAsync();
                result.GrowerAreaRecordList.AddRange(growerAreaRecord);
                //var grower = await _growerRepository.GetAll().Where(v => v.Id == entity.GrowerId).AsNoTracking().FirstOrDefaultAsync();
                //result.GrowerList.Add(grower);
            }
            foreach (var item in stIds) //同步计划任务
            {
                result.ScheduleTaskList.Add(await _scheduletaskRepository.GetAll().Where(v => v.Id == item).AsNoTracking().FirstOrDefaultAsync());
            }
            foreach (var item in vIds) // 同步任务信息and任务考核项
            {
                result.VisitTaskList.Add(await _visitTaskRepository.GetAll().Where(v => v.Id == item).AsNoTracking().FirstOrDefaultAsync());
                result.TaskExamineList.AddRange(await _taskexamineRepository.GetAll().Where(v => v.TaskId == item).AsNoTracking().ToListAsync());
            }
            //同步烟农
            result.GrowerList.AddRange(await _growerRepository.GetAll().Where(v => v.EmployeeId == userId).AsNoTracking().ToListAsync());
            //同步采集位置
            result.GrowerLocationLogList.AddRange(await _growerLocationLogRepository.GetAll().Where(v => v.EmployeeId == userId).AsNoTracking().ToListAsync());
            //同步部分系统配置
            result.SystemDataList.AddRange(await _systemdataRepository.GetAll().Where(v => v.Type == ConfigType.烟叶公共 && v.ModelId == ConfigModel.烟叶服务).ToListAsync());
            return result;
        }

        /// <summary>
        /// 离线端上传数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<APIResultDto> UploadDataAsnyc(AppUploadDto input)
        {
            foreach (var item in input.ScheduleDetailList)
            {
                var sd = await _scheduleDetailRepository.GetAll().Where(v => v.Id == item.Id).FirstOrDefaultAsync();
                if (sd != null)
                {
                    sd.CompleteNum = item.CompleteNum;
                    sd.Status = item.Status;
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }

            foreach (var item in input.GrowerAreaRecordList)
            {
                var gar = await _growerAreaRecordRepository.GetAll().Where(v => v.Id == item.Id).FirstOrDefaultAsync();
                if (gar == null)
                {
                    //await _growerAreaRecordRepository.DeleteAsync(gar.Id);
                    //await CurrentUnitOfWork.SaveChangesAsync();
                    string[] imgArry = item.ImgPath.Split(',');
                    item.ImgPath = "";
                    foreach (var img in imgArry)
                    {
                        var base64 = new ImgBase64() { imageBase64 = img };
                        var photoUrl = await FilesPostsBase64(base64);
                        item.ImgPath = string.Join(',', photoUrl);
                    }
                    await _growerAreaRecordRepository.InsertAsync(item);
                }
            }

            foreach (var item in input.VisitRecordList)
            {
                var vr = await _visitRecordRepository.GetAll().Where(v => v.Id == item.Id).FirstOrDefaultAsync();
                if (vr == null)
                {
                    await _visitRecordRepository.InsertAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }

            foreach (var item in input.VisitExamineList)
            {
                var ve = await _visitexamineRepository.GetAll().Where(v => v.Id == item.Id).FirstOrDefaultAsync();
                if (ve == null)
                {
                    await _visitexamineRepository.InsertAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }

            foreach (var item in input.GrowerList)
            {
                var g = await _growerRepository.GetAll().Where(v => v.Id == item.Id).FirstOrDefaultAsync();
                if (g != null)
                {
                    g.ActualArea = item.ActualArea;
                    g.AreaTime = item.AreaTime;
                    g.AreaStatus = item.AreaStatus;
                    g.AreaScheduleDetailId = item.AreaScheduleDetailId;
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
            return new APIResultDto() { Code = 901, Msg = "上传成功" };
        }

        /// <summary>
        /// base64转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> FilesPostsBase64(ImgBase64 input)
        {
            var saveUrl = "";
            if (!string.IsNullOrWhiteSpace(input.imageBase64))
            {
                var reg = new Regex("data:image/(.*);base64,");
                input.imageBase64 = reg.Replace(input.imageBase64, "");
                byte[] imageByte = Convert.FromBase64String(input.imageBase64);
                var memorystream = new MemoryStream(imageByte);

                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string fileExt = Path.GetExtension(input.fileName); //文件扩展名，不含“.”
                string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                var fileDire = webRootPath + "/visit/";

                if (!Directory.Exists(fileDire))
                {
                    Directory.CreateDirectory(fileDire);
                }

                var filePath = fileDire + newFileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await memorystream.CopyToAsync(stream);
                    //TODO水印
                }
                saveUrl = filePath.Substring(webRootPath.Length);
                return saveUrl;
            }
            return saveUrl;
        }
    }
}