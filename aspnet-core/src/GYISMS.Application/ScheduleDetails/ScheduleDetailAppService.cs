
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

using GYISMS.ScheduleDetails.Authorization;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleDetails;
using GYISMS.Authorization;
using GYISMS.Growers.Dtos;
using GYISMS.GYEnums;
using GYISMS.Growers;
using GYISMS.Schedules;
using GYISMS.Dtos;
using GYISMS.VisitTasks;
using Abp.Auditing;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.Helpers;
using GYISMS.DingDing;
using GYISMS.DingDing.Dtos;

namespace GYISMS.ScheduleDetails
{
    /// <summary>
    /// ScheduleDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]

    public class ScheduleDetailAppService : GYISMSAppServiceBase, IScheduleDetailAppService
    {
        //private readonly IRepository<ScheduleDetail, Guid> _scheduledetailRepository;
        private readonly IScheduleDetailManager _scheduledetailManager;
        private readonly IRepository<Grower, int> _growerRepository;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly ISheduleDetailRepository _scheduledetailRepository;
        private readonly IRepository<VisitTask, int> _visittaskRepository;
        private readonly IDingDingAppService _dingDingAppService;

        private string accessToken;
        private DingDingAppConfig ddConfig;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleDetailAppService(//IRepository<ScheduleDetail, Guid> scheduledetailRepository
           ISheduleDetailRepository scheduledetailRepository, IScheduleDetailManager scheduledetailManager, IRepository<Grower, int> growerRepository,
            IRepository<Schedule, Guid> scheduleRepository, IRepository<VisitTask, int> visittaskRepository, IDingDingAppService dingDingAppService)
        {
            _growerRepository = growerRepository;
            _scheduledetailRepository = scheduledetailRepository;
            _scheduledetailManager = scheduledetailManager;
            _growerRepository = growerRepository;
            _scheduleRepository = scheduleRepository;
            _visittaskRepository = visittaskRepository;
            _dingDingAppService = dingDingAppService;
            ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.任务拜访);
            accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
        }


        /// <summary>
        /// 获取ScheduleDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ScheduleDetailListDto>> GetPagedScheduleDetails(GetScheduleDetailsInput input)
        {

            var query = _scheduledetailRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var scheduledetailCount = await query.CountAsync();

            var scheduledetails = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var scheduledetailListDtos = ObjectMapper.Map<List <ScheduleDetailListDto>>(scheduledetails);
            var scheduledetailListDtos = scheduledetails.MapTo<List<ScheduleDetailListDto>>();

            return new PagedResultDto<ScheduleDetailListDto>(
                    scheduledetailCount,
                    scheduledetailListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取ScheduleDetailListDto信息
        /// </summary>
        public async Task<ScheduleDetailListDto> GetScheduleDetailByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _scheduledetailRepository.GetAsync(input.Id);

            return entity.MapTo<ScheduleDetailListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetScheduleDetailForEditOutput> GetScheduleDetailForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetScheduleDetailForEditOutput();
            ScheduleDetailEditDto scheduledetailEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);

                scheduledetailEditDto = entity.MapTo<ScheduleDetailEditDto>();

                //scheduledetailEditDto = ObjectMapper.Map<List <scheduledetailEditDto>>(entity);
            }
            else
            {
                scheduledetailEditDto = new ScheduleDetailEditDto();
            }

            output.ScheduleDetail = scheduledetailEditDto;
            return output;
        }

        /// <summary>
        /// 新增ScheduleDetail
        /// </summary>
        protected virtual async Task<ScheduleDetailEditDto> CreateScheduleDetailAsync(ScheduleDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<ScheduleDetail>(input);

            entity = await _scheduledetailRepository.InsertAsync(entity);
            return entity.MapTo<ScheduleDetailEditDto>();
        }

        /// <summary>
        /// 编辑ScheduleDetail
        /// </summary>
        protected virtual async Task<ScheduleDetailEditDto> UpdateScheduleDetailAsync(ScheduleDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);
            entity.VisitNum = input.VisitNum;

            // ObjectMapper.Map(input, entity);
            entity = await _scheduledetailRepository.UpdateAsync(entity);
            return entity.MapTo<ScheduleDetailEditDto>();
        }
        //protected virtual async Task<GrowerListDto> UpdateScheduleDetailAsync(ScheduleDetailEditDto input)
        //{
        //    //TODO:更新前的逻辑判断，是否允许更新

        //    var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);
        //    input.MapTo(entity);

        //    // ObjectMapper.Map(input, entity);
        //    entity = await _scheduledetailRepository.UpdateAsync(entity);
        //    var result = entity.MapTo<ScheduleDetailEditDto>();
        //    GrowerListDto growerDto = new GrowerListDto();
        //    growerDto.MapTo<ScheduleDetailEditDto>();
        //    return 
        //}



        /// <summary>
        /// 删除ScheduleDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteScheduleDetail(Guid Id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _scheduledetailRepository.DeleteAsync(Id);
        }



        /// <summary>
        /// 批量删除ScheduleDetail的方法
        /// </summary>
        public async Task BatchDeleteScheduleDetailsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _scheduledetailRepository.DeleteAsync(s => input.Contains(s.Id));
        }



        public async Task<List<ScheduleDetailEditDto>> CreateOrUpdateScheduleTaskAsync(List<ScheduleDetailEditDto> input)
        {
            List<ScheduleDetailEditDto> list = new List<ScheduleDetailEditDto>();
            //更新前删除逻辑
            var unChecked = input.Where(v => v.Id.HasValue && v.Checked == false).Select(v => new { v.EmployeeId, v.TaskId, v.ScheduleId, v.ScheduleTaskId, v.AreaCode, v.GrowerId }).ToList();
            if (unChecked.Count != 0)
            {
                foreach (var item in unChecked)
                {
                    Guid id = await _scheduledetailRepository.GetAll()
                        .Where(v => v.TaskId == item.TaskId && v.ScheduleId == item.ScheduleId && v.ScheduleTaskId == item.ScheduleTaskId && v.EmployeeId == item.EmployeeId && v.GrowerId == item.GrowerId).Select(v => v.Id).FirstOrDefaultAsync();
                    string emptyId = "{00000000-0000-0000-0000-000000000000}";
                    if (id != new Guid(emptyId))
                    {
                        await DeleteScheduleDetail(id);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }

            foreach (var item in input.Where(v => v.Checked == true))
            {
                if (item.Id.HasValue)
                {
                    list.Add(await UpdateScheduleDetailAsync(item));
                }
                else
                {
                    list.Add(await CreateScheduleDetailAsync(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取任务完成情况数据统计
        /// </summary>
        /// <returns></returns>
        public HomeInfo GetHomeInfo()
        {
            var homeInfo = new HomeInfo();
            //var aa = await _scheduledetailRepository.GetAll().ToListAsync();
            var query = from sd in _scheduledetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll().Where(s => s.Status == ScheduleMasterStatusEnum.已发布) on sd.ScheduleId equals s.Id
                        select sd;
            var totalCount = query.Sum(s => s.VisitNum);
            homeInfo.Total = totalCount.HasValue ? totalCount.Value : 0;
            var compCount = query.Sum(s => s.CompleteNum);
            homeInfo.Completed = compCount.HasValue ? compCount.Value : 0;
            if (!compCount.HasValue)
            {
                homeInfo.CompletedRate = "0%";
            }
            else
            {
                homeInfo.CompletedRate = (Math.Round((double)homeInfo.Completed / homeInfo.Total, 2) * 100).ToString() + "%";
            }
            var expirCount = query.Where(s => s.Status == ScheduleStatusEnum.已逾期).Sum(s => s.VisitNum - s.CompleteNum);
            homeInfo.Expired = expirCount.HasValue ? expirCount.Value : 0;
            return homeInfo;
        }

        /// <summary>
        /// 按区域时间统计计划完成的情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SheduleStatisticalDto>> GetSchedulByAreaTime(ScheduleDetaStatisticalInput input)
        {
            var timeNow = DateTime.Today;
            input.startTime = input.startTime.HasValue ? input.startTime : timeNow.AddDays(1 - timeNow.Day);
            input.endTime = input.endTime.HasValue ? input.endTime : timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            var query = from sd in _scheduledetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll().Where(s => s.BeginTime >= input.startTime && s.BeginTime <= input.endTime).Where(s=>s.Status== ScheduleMasterStatusEnum.已发布) on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id into sg
                        from wr in sg.DefaultIfEmpty()
                        select new
                        {
                            sd.Id,
                            sd.Status,
                            sd.VisitNum,
                            sd.CompleteNum,
                            wr.CountyCode,
                        };
            var list = await query.GroupBy(s => s.CountyCode).Select(g => new SheduleStatisticalDto
            {
                GroupName = g.Key.ToString(),
                Total = g.Sum(m => m.VisitNum),
                Completed = g.Sum(m => m.CompleteNum),
                Expired = g.Where(m => m.Status == ScheduleStatusEnum.已逾期).Sum(s => s.VisitNum - s.CompleteNum)
            }).ToListAsync();
            return list;
        }

        /// <summary>
        /// 按月份统计计划完成情况
        /// </summary>
        /// <param name="searchMoth">半年或一年</param>
        /// <returns></returns>
        public async Task<List<SheduleStatisticalDto>> GetSchedulByMothTime(int searchMoth)
        {

            var timeNow = DateTime.Today;
            DateTime startTime;
            DateTime endTime;
            if (searchMoth == 2)
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-11);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }
            else
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-5);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }

            //var query = from sd in _scheduledetailRepository.GetAll()
            //            join s in _scheduleRepository.GetAll().Where(s => s.BeginTime >= startTime && s.EndTime <= endTime) on sd.ScheduleId equals s.Id
            //            select new
            //            {
            //                sd.Id,
            //                sd.Status,
            //                sd.VisitNum,
            //                sd.CompleteNum,
            //                s.BeginTime,
            //                s.EndTime
            //            };
            //var cList = _scheduleRepository.GetAll().ToList();
            //var schList = _scheduleRepository.GetAll().Where(s => s.BeginTime >= startTime && s.EndTime <= endTime).ToList();
            //var lists = query.ToList();
            //var list = await query.GroupBy(s => new { s.BeginTime }).Select(g => new SheduleStatisticalDto
            //{
            //    //GroupName = g.Key.Month.ToString() + "份",
            //    Total = g.Sum(s => s.VisitNum),
            //    Completed = g.Sum(s => s.CompleteNum),
            //    Expired = g.Sum(s => s.VisitNum - s.CompleteNum) 
            //}).ToListAsync();
            //var result = new SheduleSumStatisDto();
            var list = await _scheduledetailRepository.GetSheduleStatisticalDtosByMothAsync(startTime, endTime);

            return list.OrderBy(s => s.GroupName).ToList();
        }

        /// <summary>
        /// 获取任务汇总数据(按区域、任务类型、任务名)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SheduleSumStatisDto> GetSumShedule(SheduleSumInput input)
        {
            var timeNow = DateTime.Today;
            //input.StartTime = input.StartTime.HasValue ? input.StartTime : timeNow.AddDays(1 - timeNow.Day);
            //input.EndTime = input.EndTime.HasValue ? input.EndTime : timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);

            var query = from sd in _scheduledetailRepository.GetAll()
                        join t in _visittaskRepository.GetAll()
                        .WhereIf(input.TaskId.HasValue, t => t.Id == input.TaskId)
                        on sd.TaskId equals t.Id
                        join s in _scheduleRepository.GetAll()
                            .WhereIf(input.StartTime.HasValue, s => s.BeginTime >= input.StartTime)
                            .WhereIf(input.EndTime.HasValue, s => s.BeginTime <= input.EndTime)
                            .Where(s=>s.Status== ScheduleMasterStatusEnum.已发布)
                            on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll()
                        .WhereIf(input.AreaCode.HasValue, g => g.CountyCode == input.AreaCode)
                        on sd.GrowerId equals g.Id
                        select new
                        {
                            g.CountyCode,
                            t.Type,
                            t.Name,
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status
                        };

            var equery = from q in query
                         group new
                         {
                             q.CountyCode,
                             q.Type,
                             q.Name,
                             q.VisitNum,
                             q.CompleteNum,
                             q.Status
                         } by new { q.CountyCode, q.Type, q.Name } into gq
                         select new SheduleSumDto()
                         {
                             AreaCode = gq.Key.CountyCode,
                             TaskType = gq.Key.Type,
                             TaskName = gq.Key.Name,
                             Total = gq.Sum(g => g.VisitNum),
                             Complete = gq.Sum(g => g.CompleteNum),
                             Expired = gq.Where(m => m.Status == ScheduleStatusEnum.已逾期).Sum(s => s.VisitNum - s.CompleteNum)
                         };

            var result = new SheduleSumStatisDto();

            var dataList = (await equery.ToListAsync()).MapTo<List<SheduleSumDto>>();
            result.sheduleSumDtos = dataList;
            var total = dataList.Sum(s => s.Total);
            result.TotalSum = total.HasValue ? total.Value : 0;
            var complete = dataList.Sum(s => s.Complete);
            result.CompleteSum = complete.HasValue ? complete.Value : 0;
            var expireds = dataList.Sum(s => s.Expired);
            result.ExpiredSum = expireds.HasValue ? expireds.Value : 0;

            return result;

            //var query = (from sd in _scheduledetailRepository.GetAll()
            //             join s in _scheduleRepository.GetAll().Where(s => s.BeginTime >= input.StartTime && s.BeginTime <= input.EndTime) on sd.ScheduleId equals s.Id
            //             join t in _visittaskRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.TaskName), t => t.Name.Contains(input.TaskName)) on sd.TaskId equals t.Id
            //             join g in _growerRepository.GetAll().WhereIf(input.Area.HasValue, g => g.CountyCode == input.Area) on sd.GrowerId equals g.Id into gs
            //             from sdg in gs.DefaultIfEmpty()
            //             select new
            //             {
            //                 sdg.CountyCode,
            //                 t.Name,
            //                 t.Type,
            //                 sd.VisitNum,
            //                 sd.CompleteNum,
            //                 sd.Status
            //             }).ToList();

            //var list = query.Select(s => new SheduleSumDto
            //{
            //    Area = s.CountyCode,
            //    TaskName = s.Name,
            //    TaskType = s.Type,
            //    Total = s.VisitNum.HasValue ? s.VisitNum.Value : 0,
            //    Complete = s.CompleteNum.HasValue ? s.VisitNum.Value : 0,
            //    Expired = s.VisitNum.HasValue ? s.VisitNum.Value : 0,
            //})

            //var list =await _scheduledetailRepository.GetSheduleSum(input.Area, input.StartTime, input.EndTime, input.TaskName);
            //return list;

        }

        public async Task<PagedResultDto<SheduleDetailTaskListDto>> GetPagedScheduleDetailsByOtherTable(GetScheduleDetailsInput input)
        {

            //var query = _scheduledetailRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var query = from sd in _scheduledetailRepository.GetAll()
                                                        .WhereIf(!string.IsNullOrEmpty(input.EmployeeName), sd => sd.EmployeeName.Contains(input.EmployeeName))
                                                        .WhereIf(!string.IsNullOrEmpty(input.GrowerName), sd => sd.GrowerName.Contains(input.GrowerName))
                        join s in _scheduleRepository.GetAll()
                                                     .WhereIf(input.StartTime.HasValue, s => s.BeginTime >= input.StartTime)
                                                     .WhereIf(input.EndTime.HasValue, s => s.BeginTime <= input.EndTime)
                                                     .Where(s=>s.Status== ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visittaskRepository.GetAll()
                                                     .WhereIf(input.TaskId.HasValue, t => t.Id == input.TaskId)
                        on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll()
                                                     .WhereIf(input.AreaCode.HasValue, g => g.CountyCode == input.AreaCode)
                        on sd.GrowerId equals g.Id
                        select new SheduleDetailTaskListDto
                        {
                            Id = sd.Id,
                            VisitNum = sd.VisitNum,
                            CompleteNum = sd.CompleteNum,
                            Status = sd.Status,
                            TaskName = t.Name,
                            TaskType = t.Type,
                            AreaCode = g.CountyCode,
                            GrowerName = sd.GrowerName,
                            EmployeeName = sd.EmployeeName
                        };

            var scheduledetailCount = await query.CountAsync();

            var scheduledetails = await query
                    //.OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var scheduledetailListDtos = ObjectMapper.Map<List <ScheduleDetailListDto>>(scheduledetails);
            var scheduledetailListDtos = scheduledetails.MapTo<List<SheduleDetailTaskListDto>>();

            return new PagedResultDto<SheduleDetailTaskListDto>(
                    scheduledetailCount,
                    scheduledetailListDtos
                );
        }

        /// 任务全部指派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<APIResultDto> CreateAllScheduleTaskAsync(GetGrowersInput input)
        {
            // 全部指派找出已存在指派信息
            var hasScheduleDetail = await _scheduledetailRepository.GetAll().Where(v => v.ScheduleTaskId == input.ScheduleTaskId).ToListAsync();
            if (hasScheduleDetail.Count != 0)
            {
                var growerIds = _growerRepository.GetAll().Where(v => v.IsDeleted == false).Select(v => v.Id);
                var sameIds = hasScheduleDetail.Where(v => growerIds.Contains(v.GrowerId)).Select(v => v.Id).ToList();
                await BatchDeleteScheduleDetailsAsync(sameIds);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            var growerList = _growerRepository.GetAll().Where(v => v.IsDeleted == false);
            foreach (var item in growerList)
            {
                ScheduleDetail entity = new ScheduleDetail();
                entity.VisitNum = input.VisitNum;
                entity.ScheduleTaskId = input.ScheduleTaskId;
                entity.CompleteNum = 0;
                entity.Status = ScheduleStatusEnum.未开始;
                entity.ScheduleId = input.ScheduleId;
                entity.TaskId = input.TaskId;
                entity.GrowerId = item.Id;
                entity.GrowerName = item.Name;
                if (item.EmployeeName != null)
                {
                    entity.EmployeeName = item.EmployeeName;
                }
                if (item.EmployeeId != null)
                {
                    entity.EmployeeId = item.EmployeeId;
                }
                var result = await _scheduledetailRepository.InsertAsync(entity);
            }
            return new APIResultDto() { Code = 0, Msg = "任务批量指派成功" };
        }

        /// <summary>
        /// 自动更新逾期计划状态
        /// </summary>
        [Audited]
        [AbpAllowAnonymous]
        public async Task AutoUpdateOverdueStatusAsync()
        {
            var dateTime = DateTime.Now;
            var query = from sd in _scheduledetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        on sd.ScheduleId equals s.Id
                        where s.Status == ScheduleMasterStatusEnum.已发布
                        && s.EndTime < dateTime        //已过期
                        && sd.CompleteNum < sd.VisitNum//未完成
                        && sd.Status != ScheduleStatusEnum.已逾期
                        select sd;
            var overdueList = await query.ToListAsync();
            foreach (var item in overdueList)
            {
                item.Status = ScheduleStatusEnum.已逾期;
            }
        }

        /// <summary>
        /// 发送任务过期提醒
        /// </summary>
        [Audited]
        [AbpAllowAnonymous]
        public async Task SendTaskOverdueMsgAsync()
        {
            var dateTime = DateTime.Today.AddDays(-1);
            var query = from sd in _scheduledetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        on sd.ScheduleId equals s.Id
                        join t in _visittaskRepository.GetAll()
                        on sd.TaskId equals t.Id
                        where s.Status == ScheduleMasterStatusEnum.已发布
                        && s.EndTime <= dateTime        //还有1天过期
                        && sd.CompleteNum < sd.VisitNum//未完成
                        && sd.Status != ScheduleStatusEnum.已逾期
                        select new
                        {
                            sd.EmployeeId,
                            sd.EmployeeName,
                            t.Name,
                            s.EndTime
                        };
            var overdueList = await query.ToListAsync();
            foreach (var item in overdueList)
            {
                //发送工作消息
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");
                OapiMessageCorpconversationAsyncsendV2Request request = new OapiMessageCorpconversationAsyncsendV2Request();
                request.UseridList = item.EmployeeId;
                request.ToAllUser = false;
                request.AgentId = ddConfig.AgentID;

                OapiMessageCorpconversationAsyncsendV2Request.MsgDomain msg = new OapiMessageCorpconversationAsyncsendV2Request.MsgDomain();
                msg.Link = new OapiMessageCorpconversationAsyncsendV2Request.LinkDomain();
                msg.Msgtype = "link";
                msg.Link.Title = "任务过期提醒";
                msg.Link.Text = string.Format("{0}：您有任务[{1}]即将过期，过期日期：{2}，点击查看详细",item.EmployeeName, item.Name, item.EndTime.Value.ToString("yyyy-MM-dd"));
                msg.Link.PicUrl = "@lALPBY0V4-AiG7vMgMyA";
                msg.Link.MessageUrl = "eapp://";
                request.Msg_ = msg;
                OapiMessageCorpconversationAsyncsendV2Response response = client.Execute(request, accessToken);
            }
        }
    }
}


