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
        /// <summary>
        /// 构造函数 
        ///</summary>
        public ChartAppService(IRepository<ScheduleTask, Guid> scheduletaskRepository
            , IRepository<Schedule, Guid> scheduleRepository
            , IRepository<VisitTask> visitTaskRepository
            , ISheduleDetailRepository scheduleDetailRepository
            , IRepository<Grower> growerRepository
            , IRepository<VisitRecord, Guid> visitRecordRepository
            )
        {
            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
            _visitRecordRepository = visitRecordRepository;

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
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "完成", ClassName = "complete", Percent = cpercent, Seq = 1 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "逾期", ClassName = "overdue", Percent = etpercent, Seq = 3 });

                //进行中数
                var pnum = tnum - cnum - etnum;
                var ppercent = 1M - cpercent - etpercent;
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "进行中", ClassName = "process", Percent = ppercent, Seq = 2 });

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
<<<<<<< HEAD
        /// 统计任务完成情况的数据（按任务类型和任务名分组）
        /// </summary>
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetChartByGroupAsync(DateTime? startTime, DateTime? endTime)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startTime.HasValue, s => s.BeginTime >= startTime)
                        .WhereIf(startTime.HasValue, s => s.BeginTime <= endTime)
                        on sd.ScheduleId equals s.Id
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        select new
                        {
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status,
                            t.Type,
                            t.Name
                        };
            var list = query.GroupBy(s => new { s.Name, s.Type }).Select(s =>
            new SheduleByTaskDto()
            {
                TaskName = s.Key.Name,
                VisitNum = s.Sum(sd => sd.VisitNum),
                CompleteNum = s.Sum(sd => sd.CompleteNum),
                ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
            });
            var result = new ChartByTaskDto();
            result.SheduleByTaskDtos = await list.ToListAsync();
            return result;
        }

        /// <summary>
        /// 按月统计任务（半年、一年）
        /// </summary>
        /// <param name="searchMoth"></param>
        /// <returns></returns>
        public async Task<List<DistrictChartItemDto>> GetChartByMothAsync(int searchMoth)
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
            foreach (var item in list)
            {
                items.Add(new DistrictChartItemDto()
                {
                    District = item.GroupName,
                    Name = "计划",
                    Num = item.Total
                });
                items.Add(new DistrictChartItemDto()
                {
                    District = item.GroupName,
                    Name = "进行中",
                    Num = item.Completed
                });
                items.Add(new DistrictChartItemDto()
                {
                    District = item.GroupName,
                    Name = "逾期",
                    Num = item.Expired
                });
            }
            return items;
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
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "完成", ClassName = "complete", Percent = cpercent, Seq = 1 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "逾期", ClassName = "overdue", Percent = etpercent, Seq = 3 });

                //进行中数
                var pnum = tnum - cnum - etnum;
                var ppercent = 1M - cpercent - etpercent;
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "进行中", ClassName = "process", Percent = ppercent, Seq = 2 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }
    }
}
