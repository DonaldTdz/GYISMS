
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
        private string accessToken;
        private DingDingAppConfig ddConfig;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleAppService(IRepository<Schedule, Guid> scheduleRepository
            , IScheduleManager scheduleManager
            , ISheduleDetailRepository scheduledetailRepository
            , IDingDingAppService dingDingAppService
            , IRepository<SystemData, int> systemdataRepository
            , IRepository<User, long> userRepository
            )
        {
            _scheduleRepository = scheduleRepository;
            _scheduleManager = scheduleManager;
            _scheduledetailRepository = scheduledetailRepository;
            _dingDingAppService = dingDingAppService;
            _systemdataRepository = systemdataRepository;
            _userRepository = userRepository;

            ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.任务拜访);
            accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
        }


        /// <summary>
        /// 获取Schedule的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ScheduleListDto>> GetPagedSchedulesAsync(GetSchedulesInput input)
        {
            var detail = _scheduledetailRepository.GetAll();
            var query = _scheduleRepository.GetAll().Where(v => v.IsDeleted == false)
                     .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                     .WhereIf(input.ScheduleType.HasValue, r => r.Type == input.ScheduleType);
            var user = _userRepository.GetAll();
            var entity = from q in query
                                join u in user on q.CreatorUserId equals u.Id into table
                                from t in table.DefaultIfEmpty()
                                select new ScheduleListDto()
                                {
                                    Id = q.Id,
                                    Name = q.Name,
                                    Type = q.Type,
                                    Status = q.Status,
                                    PublishTime = q.PublishTime,
                                    CreateUserName = t.Name,
                                    Area = t.Area != null ? t.Area : " - "
                                };

           var temp = await (from d in detail
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
                                  VisitCount = g.Sum(v=>v.VisitNum),
                              }).ToListAsync();
            var result = (from e in entity
                    join t in temp on e.Id equals t.Id into g
                    from table in g.DefaultIfEmpty()
                    select new ScheduleListDto() {
                        Id = e.Id,
                        Name = e.Name,
                        Type = e.Type,
                        Status = e.Status,
                        PublishTime = e.PublishTime,
                        CreateUserName = e.Name,
                        Area = e.Area,
                        Percentage = table != null ? Math.Round((Convert.ToDecimal((double)table.CompleteCount/table.VisitCount)),2) : 0
                    });

            var scheduleCount = await query.CountAsync();

            var schedules = await result
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

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
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Schedule>(input);
            entity.IsDeleted = false;
            var id = await _scheduleRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<ScheduleEditDto>();
        }

        /// <summary>
        /// 编辑Schedule
        /// </summary>
        protected virtual async Task<ScheduleEditDto> UpdateScheduleAsync(ScheduleEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

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
            var entity = await _scheduleRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _scheduleRepository.UpdateAsync(entity);
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
        /// <returns></returns>
        public object UpdateAndGetMediaId(string path)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/media/upload");
            OapiMediaUploadRequest request = new OapiMediaUploadRequest();
            request.Type = "image";
            request.Media = new Top.Api.Util.FileItem($@"{path}");
            OapiMediaUploadResponse response = client.Execute(request, accessToken);
            return response;
        }


        /// <summary>
        /// 发送钉钉工作通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<APIResultDto> SendMessageToEmployeeAsync(GetSchedulesInput input)
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
            for (int i = 0; i < Math.Ceiling(frequency); i++)
            {
                var temp = ids.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                string tempIds = string.Join(",", temp.ToArray());
                //发送工作消息
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");
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
                OapiMessageCorpconversationAsyncsendV2Response response = client.Execute(request, accessToken);
                pageIndex++;
            }
            return new APIResultDto() { Code = 0, Msg = "钉钉消息发送成功" };
        }
    }
}