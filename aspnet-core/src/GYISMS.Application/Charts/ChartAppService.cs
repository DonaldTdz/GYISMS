using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using GYISMS.Charts.Dtos;
using GYISMS.Growers;
using GYISMS.ScheduleDetails;
using GYISMS.Schedules;
using GYISMS.ScheduleTasks;
using GYISMS.VisitRecords;
using GYISMS.VisitTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GYISMS.GYEnums;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Uow;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using GYISMS.Employees;

namespace GYISMS.Charts
{
    [AbpAllowAnonymous]
    [DisableAuditing]
    [UnitOfWork(isTransactional: false)]
    public class ChartAppService : GYISMSAppServiceBase, IChartAppService
    {
        private readonly IRepository<ScheduleTask, Guid> _scheduletaskRepository;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        //private readonly IRepository<ScheduleDetail, Guid> _scheduleDetailRepository;
        private readonly IRepository<VisitTask> _visitTaskRepository;
        private readonly IRepository<Grower> _growerRepository;
        private readonly IRepository<VisitRecord, Guid> _visitRecordRepository;
        private readonly ISheduleDetailRepository _scheduleDetailRepository;
        private readonly IRepository<VisitTask, int> _visittaskRepository;
        private readonly IEmployeeManager _employeeManager;
        /// <summary>
        /// 构造函数 
        ///</summary>
        public ChartAppService(IRepository<ScheduleTask, Guid> scheduletaskRepository
            , IRepository<Schedule, Guid> scheduleRepository
            , IRepository<VisitTask> visitTaskRepository
            , ISheduleDetailRepository scheduleDetailRepository
            , IRepository<Grower> growerRepository
            , IRepository<VisitRecord, Guid> visitRecordRepository
            , IRepository<VisitTask, int> visittaskRepository
            , IEmployeeManager employeeManager
            )
        {
            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
            _visitRecordRepository = visitRecordRepository;
            _visittaskRepository = visittaskRepository;
            _employeeManager = employeeManager;
        }

        /// <summary>
        /// 计划汇总
        /// </summary>
        public async Task<List<ScheduleSummaryDto>> GetScheduleSummaryAsync(string userId, AreaCodeEnum areaCode)
        {
            //获取当前用户区县
            //var area = await _employeeManager.GetAreaCodeByUserIdAsync(userId);
            return await Task.Run(() =>
            {
                var dataList = new List<ScheduleSummaryDto>();
                var query = from sd in _scheduleDetailRepository.GetAll()
                            join s in _scheduleRepository.GetAll() on sd.ScheduleId equals s.Id
                            join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                            where s.Status == ScheduleMasterStatusEnum.已发布
                            && (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)//区县数据过滤
                            && s.EndTime >= DateTime.Today
                            select sd;
                //总数
                var tnum = query.Sum(q => q.VisitNum);
                tnum = tnum ?? 0;
                //完成数
                var cnum = query.Sum(q => q.CompleteNum);
                cnum = cnum ?? 0;
                var cpercent = tnum == 0 ? 0 : Math.Round(cnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "完成", ClassName = "complete", Percent = cpercent * 100, Seq = 1, Status = 2 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = tnum == 0 ? 0 : Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "逾期", ClassName = "overdue", Percent = etpercent * 100, Seq = 3, Status = 0 });

                //待完成数
                var pnum = tnum - cnum - etnum;
                var ppercent = tnum == 0 ? 0 : (1M - cpercent - etpercent);
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "待完成", ClassName = "process", Percent = ppercent * 100, Seq = 2, Status = 3 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }


        /// <summary>
        /// 获取区县图表数据
        /// </summary>
        /// <param name="tabIndex">（1表示当前任务，结束时间大于今天；2表示所有任务）</param>
        /// <returns></returns>
        public async Task<DistrictChartDto> GetDistrictChartDataAsync(string userId, DateTime? startDate, DateTime? endDate, int tabIndex, AreaCodeEnum areaCode)
        {
            //var str = startDate.Value.ToString("yyyy-MM-dd HH:mm ss");
            //if (startDate.HasValue)
            //{
            //    var sdate = startDate.Value.Date;
            //    startDate = sdate.AddHours(sdate.Hour * -1);
            //}
            //if (endDate.HasValue)
            //{
            //    var edate = endDate.Value.Date;
            //    endDate = edate.AddHours(edate.Hour * -1);
            //}
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startDate.HasValue && tabIndex == 2, s => s.EndTime >= startDate)
                        .WhereIf(endDate.HasValue && tabIndex == 2, s => s.EndTime <= endDate)
                        .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Now)
                        on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                        where s.Status == ScheduleMasterStatusEnum.已发布
                        where (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)//添加区县权限 add by donald 2019-1-22
                        select new
                        {
                            g.AreaCode,
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status
                        };
            var rquery = query.GroupBy(g => g.AreaCode).Select(g1 =>
                         new DistrictDto()
                         {
                             AreaType = g1.Key,
                             VisitNum = g1.Sum(g => g.VisitNum),
                             CompleteNum = g1.Sum(g => g.CompleteNum),
                             OverdueNum = g1.Where(g => g.Status == ScheduleStatusEnum.已逾期).Sum(g => g.VisitNum - g.CompleteNum)
                         });

            var resultData = new DistrictChartDto();
            resultData.Districts = await rquery.ToListAsync();
            return resultData;
        }

        /// <summary>
        /// 统计任务完成情况的数据（按任务类型和任务名分组）
        /// </summary>
        /// <param name="tabIndex">（1表示当前任务，结束时间大于今天；2表示所有任务）</param>
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetChartByGroupAsync(DateTime? startTime, DateTime? endTime, int tabIndex, AreaCodeEnum areaCode)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime >= startTime)
                        .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
                        .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                        .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                        where (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)//添加区县权限 add by donald 2019-1-22
                        select new
                        {
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status,
                            t.Type,
                            t.Name,
                            t.Id
                        };
            var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
             new SheduleByTaskDto()
             {
                 Id = s.Key.Id,
                 //TaskType=s.Key.Type,
                 TaskName = s.Key.Name,
                 VisitNum = s.Sum(sd => sd.VisitNum),
                 CompleteNum = s.Sum(sd => sd.CompleteNum),
                 ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
             });
            var result = new ChartByTaskDto();
            result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
            return result;
        }

        /// <summary>
        /// 按月统计任务（半年、一年）
        /// </summary>
        /// <param name="searchMoth"></param>
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetChartByMothAsync(int searchMoth, AreaCodeEnum areaCode)
        {
            var timeNow = DateTime.Today;
            DateTime startTime;
            DateTime endTime;
            if (searchMoth == 1)
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-11);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }
            else
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-5);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }
            var list = await _scheduleDetailRepository.GetSheduleStatisticalDtosByMothAsync(startTime, endTime, areaCode);
            list.OrderBy(s => s.GroupName).ToList();
            var items = new List<DistrictChartItemDto>();
            var result = new ChartByTaskDto();
            foreach (var item in list)
            {
                result.Tasks.Add(new SheduleByTaskDto
                {
                    TaskName = item.GroupName,
                    VisitNum = item.Total,
                    CompleteNum = item.Completed,
                    ExpiredNum = item.Expired
                });
                //items.Add(new DistrictChartItemDto()
                //{
                //    District = item.GroupName,
                //    Name = "计划",
                //    Num = item.Total
                //});
                //items.Add(new DistrictChartItemDto()
                //{
                //    District = item.GroupName,
                //    Name = "进行中",
                //    Num = item.Completed
                //});
                //items.Add(new DistrictChartItemDto()
                //{
                //    District = item.GroupName,
                //    Name = "逾期",
                //    Num = item.Expired
                //});
            }
            return result;
        }

        /// <summary>
        /// 个人计划汇总
        /// </summary>
        public async Task<List<ScheduleSummaryDto>> GetUserScheduleSummaryAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var dataList = new List<ScheduleSummaryDto>();
                var query = from sd in _scheduleDetailRepository.GetAll()
                            join s in _scheduleRepository.GetAll() on sd.ScheduleId equals s.Id
                            where s.Status == ScheduleMasterStatusEnum.已发布
                            && sd.EmployeeId == userId && s.EndTime >= DateTime.Today
                            select sd;
                //总数
                var tnum = query.Sum(q => q.VisitNum);
                tnum = tnum ?? 0;
                //完成数
                var cnum = query.Sum(q => q.CompleteNum);
                cnum = cnum ?? 0;
                var cpercent = tnum == 0 ? 0 : Math.Round(cnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "已完成", ClassName = "complete", Percent = cpercent * 100, Seq = 1 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = tnum == 0 ? 0 : Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "已逾期", ClassName = "overdue", Percent = etpercent * 100, Seq = 3 });

                //待完成数
                var pnum = tnum - cnum - etnum;
                var ppercent = tnum == 0 ? 0 : (1M - cpercent - etpercent);
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "待完成", ClassName = "process", Percent = ppercent * 100, Seq = 2 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }

        /// <summary>
        /// 获取任务的明细信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<SheduleDetailDto>> GetSheduleDetail(int PageIndex, int TabIndex, string DateString, AreaCodeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus)
        {
            if (!string.IsNullOrEmpty(DateString))
            {
                var str = DateString.Split("-");
                StartTime = new DateTime(int.Parse(str[0]), int.Parse(str[1]), 1);
                EndTime = StartTime.Value.AddMonths(1).AddDays(-1);
            }
            var querysd = _scheduleDetailRepository.GetAll();
            //完成
            if (Status == 2)
            {
                querysd = querysd.Where(sd => sd.CompleteNum > 0);

            } //逾期
            else if (Status == 0)
            {
                querysd = querysd.Where(sd => sd.Status == ScheduleStatusEnum.已逾期);
            }
            ////进行中
            //if (TStatus == 1)
            //{
            //    querysd = querysd.Where(sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum);
            //}
           
            var query = from sd in _scheduleDetailRepository.GetAll()
                                                      .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                                                      .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                                                      .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                        join s in _scheduleRepository.GetAll()
                                                      .WhereIf(StartTime.HasValue && (TabIndex == 2 || TabIndex == 0), s => s.EndTime >= StartTime)
                                                      .WhereIf(EndTime.HasValue && (TabIndex == 2 || TabIndex == 0), s => s.EndTime <= EndTime)
                                                      .WhereIf(TabIndex == 1, s => s.EndTime >= DateTime.Today)
                                                      .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visittaskRepository.GetAll()
                                                      //.WhereIf(input.TaskType.HasValue, t => t.Type == input.TaskType)
                                                      //.WhereIf(!string.IsNullOrEmpty(input.TaskName), t => t.Name == input.TaskName)
                                                      .WhereIf(TaskId.HasValue, t => t.Id == TaskId)
                         on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll()
                                                   .WhereIf(AreaCode.HasValue, g => g.AreaCode == AreaCode)
                        on sd.GrowerId equals g.Id
                        select new SheduleDetailDto
                        {
                            Id = sd.Id,
                            AreaCode = g.AreaCode,
                            TaskType = t.Type,
                            TaskName = t.Name,
                            BeginTime = s.BeginTime,
                            EndTime = s.EndTime,
                            Status = sd.Status,
                            EmployeeName = sd.EmployeeName,
                            GrowerName = sd.GrowerName,
                            VisitNum = sd.VisitNum,
                            CompleteNum = sd.CompleteNum
                        };
            var items = await query.OrderByDescending(s => s.BeginTime).Skip(PageIndex).Take(10).ToListAsync();
            return items;
        }

        /// <summary>
        /// 获取明细区县统计
        /// </summary>
        /// <param name="Status">(0表示逾期、2表示完成、3表示待完成)</param>
        /// <param name="TabIndex">(0当前用于月份统计、1表示当前任务，结束时间大于今天；2表示所有任务)</param>
        /// <returns></returns>
        public async Task<List<DistrictStatisDto>> GetSheduleDetailGroupArea(string DateString, int TabIndex, AreaCodeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus)
        {
            if (!string.IsNullOrEmpty(DateString))
            {
                var str = DateString.Split("-");
                StartTime = new DateTime(int.Parse(str[0]), int.Parse(str[1]), 1);
                EndTime = StartTime.Value.AddMonths(1).AddDays(-1);
            }
            var query = from sd in _scheduleDetailRepository.GetAll()
                                                     .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                                                     .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                                                     .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                        join s in _scheduleRepository.GetAll()
                                                      .WhereIf(StartTime.HasValue && (TabIndex == 2 || TabIndex == 0), s => s.EndTime >= StartTime)
                                                      .WhereIf(EndTime.HasValue && (TabIndex == 2 || TabIndex == 0), s => s.EndTime <= EndTime)
                                                      .WhereIf(TabIndex == 1, s => s.EndTime >= DateTime.Today)
                                                      .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visittaskRepository.GetAll()
                                                      //.WhereIf(input.TaskType.HasValue, t => t.Type == input.TaskType)
                                                      //.WhereIf(!string.IsNullOrEmpty(input.TaskName), t => t.Name == input.TaskName)
                                                      .WhereIf(TaskId.HasValue, t => t.Id == TaskId)
                         on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll()
                                                     .WhereIf(AreaCode.HasValue, g => g.AreaCode == AreaCode)
                        on sd.GrowerId equals g.Id
                        select new SheduleDetailDto
                        {
                            AreaCode = g.AreaCode,
                            Status = sd.Status,
                            EmployeeName = sd.EmployeeName,
                            GrowerName = sd.GrowerName,
                            VisitNum = sd.VisitNum,
                            CompleteNum = sd.CompleteNum
                        };
            var result = await query.GroupBy(s => new { s.AreaCode }).Select(s => new DistrictStatisDto
            {
                Status = Status,
                AreaCode = s.Key.AreaCode,
                VisitNum = s.Sum(sd => sd.VisitNum),
                CompleteNum = s.Sum(sd => sd.CompleteNum),
                ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
            }).ToListAsync();
            return result;
        }
    }
}
