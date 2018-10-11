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
            )
        {
            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
            _visitRecordRepository = visitRecordRepository;
            _visittaskRepository = visittaskRepository;
        }

        /// <summary>
        /// 计划汇总
        /// </summary>
        public async Task<List<ScheduleSummaryDto>> GetScheduleSummaryAsync(string userId)
        {
            return await Task.Run(() =>
            {
                var dataList = new List<ScheduleSummaryDto>();
                var query = from sd in _scheduleDetailRepository.GetAll()
                            join s in _scheduleRepository.GetAll() on sd.ScheduleId equals s.Id
                            where s.Status == ScheduleMasterStatusEnum.已发布
                            select sd;
                //总数
                var tnum = query.Sum(q => q.VisitNum);
                tnum = tnum ?? 0;
                //完成数
                var cnum = query.Sum(q => q.CompleteNum);
                cnum = cnum ?? 0;
                var cpercent = Math.Round(cnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "完成", ClassName = "complete", Percent = cpercent*100, Seq = 1,Status=2 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "逾期", ClassName = "overdue", Percent = etpercent*100, Seq = 3,Status=0 });

                //进行中数
                var pnum = tnum - cnum - etnum;
                var ppercent = 1M - cpercent - etpercent;
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "进行中", ClassName = "process", Percent = ppercent*100, Seq = 2 ,Status=3});

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }

        /// <summary>
        /// 获取区县图表数据
        /// </summary>
        public async Task<DistrictChartDto> GetDistrictChartDataAsync(string userId, DateTime? startDate, DateTime? endDate)
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
                        .WhereIf(startDate.HasValue, s => s.BeginTime >= startDate)
                        .WhereIf(endDate.HasValue, s => s.BeginTime <= endDate)
                        on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                        where s.Status == ScheduleMasterStatusEnum.已发布
                        select new
                        {
                            g.CountyCode,
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status
                        };
            var rquery = query.GroupBy(g => g.CountyCode).Select(g1 =>
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
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetChartByGroupAsync(DateTime? startTime, DateTime? endTime)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startTime.HasValue, s => s.BeginTime >= startTime)
                        .WhereIf(startTime.HasValue, s => s.BeginTime <= endTime)
                        .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
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
            result.Tasks = await list.ToListAsync();
            return result;
        }

        /// <summary>
        /// 按月统计任务（半年、一年）
        /// </summary>
        /// <param name="searchMoth"></param>
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetChartByMothAsync(int searchMoth)
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
            var list = await _scheduleDetailRepository.GetSheduleStatisticalDtosByMothAsync(startTime, endTime);
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
                            && sd.EmployeeId == userId
                            select sd;
                //总数
                var tnum = query.Sum(q => q.VisitNum);
                tnum = tnum ?? 0;
                //完成数
                var cnum = query.Sum(q => q.CompleteNum);
                cnum = cnum ?? 0;
                var cpercent = Math.Round(cnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "已完成", ClassName = "complete", Percent = cpercent*100, Seq = 1 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "已逾期", ClassName = "overdue", Percent = etpercent*100, Seq = 3 });

                //进行中数
                var pnum = tnum - cnum - etnum;
                var ppercent = 1M - cpercent - etpercent;
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "进行中", ClassName = "process", Percent = ppercent*100, Seq = 2 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }

        /// <summary>
        /// 获取任务的明细信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<SheduleDetailDto>> GetSheduleDetail(int PageIndex, string DateString, AreaTypeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus)
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
            var list = querysd.ToList();
            var query = from sd in _scheduleDetailRepository.GetAll()
                                                      .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                                                      .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                                                      .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                        join s in _scheduleRepository.GetAll()
                                                      .WhereIf(StartTime.HasValue, s => s.BeginTime >= StartTime)
                                                      .WhereIf(EndTime.HasValue, s => s.BeginTime <= EndTime)
                                                      .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visittaskRepository.GetAll()
                                                      //.WhereIf(input.TaskType.HasValue, t => t.Type == input.TaskType)
                                                      //.WhereIf(!string.IsNullOrEmpty(input.TaskName), t => t.Name == input.TaskName)
                                                      .WhereIf(TaskId.HasValue, t => t.Id == TaskId)
                         on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll()
                                                     .WhereIf(AreaCode.HasValue, g => g.CountyCode == AreaCode)
                        on sd.GrowerId equals g.Id
                        select new SheduleDetailDto
                        {
                            Id = sd.Id,
                            AreaCode = g.CountyCode,
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
            var items = await query.OrderByDescending(s => s.BeginTime).Skip(PageIndex).Take(9).ToListAsync();
            return items;
        }
    }
}
