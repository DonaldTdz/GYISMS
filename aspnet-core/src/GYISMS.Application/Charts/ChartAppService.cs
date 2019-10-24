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
using GYISMS.Organizations;
using GYISMS.SystemDatas;

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
        private readonly IRepository<SystemData> _systemdataRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Organization, long> _organizationRepository;
        /// <summary>
        /// 构造函数 
        ///</summary>
        public ChartAppService(IRepository<ScheduleTask, Guid> scheduletaskRepository
            , IRepository<Schedule, Guid> scheduleRepository
            , IRepository<VisitTask> visitTaskRepository
            , ISheduleDetailRepository scheduleDetailRepository
            , IRepository<Grower> growerRepository
            , IRepository<VisitRecord, Guid> visitRecordRepository
            , IRepository<VisitTask> visittaskRepository
            , IEmployeeManager employeeManager
            , IRepository<SystemData> systemdataRepository
            , IRepository<Employee, string> employeeRepository
            , IRepository<Organization, long> organizationRepository
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
            _systemdataRepository = systemdataRepository;
            _employeeRepository = employeeRepository;
            _organizationRepository = organizationRepository;
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
                var cnum = query.Sum(q => q.CompleteNum - q.VisitNum > 0 ? q.VisitNum : q.CompleteNum);
                cnum = cnum ?? 0;
                var cpercent = tnum == 0 ? 0 : Math.Round(cnum.Value / (decimal)tnum.Value, 2); //百分比
                //var cpercent = Convert.ToDecimal(cnum.Value) / Convert.ToDecimal(tnum.Value);
                //var cpercent = percent.ToString("0.00%");//得到5.88%
                dataList.Add(new ScheduleSummaryDto() { Num = cnum, Name = "完成", ClassName = "complete", Percent = cpercent * 100, Seq = 1, Status = 2 });

                //逾期数
                var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                etnum = etnum ?? 0;
                var etpercent = tnum == 0 ? 0 : Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "逾期", ClassName = "overdue", Percent = etpercent * 100, Seq = 3, Status = 0 });

                //待完成数
                //var pnum = tnum - cnum - etnum;
                //var ppercent = tnum == 0 ? 0 : (1M - cpercent - etpercent);
                var pnum = query.Sum(q => q.VisitNum - q.CompleteNum >= 0 ? q.VisitNum - q.CompleteNum : 0);
                var ppercent = tnum == 0 ? 0 : Math.Round(pnum.Value / (decimal)tnum.Value, 2);
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "待完成", ClassName = "process", Percent = ppercent * 100, Seq = 2, Status = 3 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }

        #region 区县图表部分

        /// <summary>
        /// 获取区县图表数据
        /// </summary>
        /// <param name="tabIndex">（1表示当前任务，结束时间大于今天；2表示所有任务）</param>
        public async Task<DistrictChartDto> GetDistrictChartDataAsync(string userId, DateTime? startDate, DateTime? endDate, int tabIndex, AreaCodeEnum areaCode)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startDate.HasValue, s => s.EndTime >= startDate)
                        .WhereIf(endDate.HasValue, s => s.EndTime <= endDate)
                        .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
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

        private async Task<string[]> GetEmployeeIdsByDeptId(long deptId)
        {
            var childrenDeptIds = await _employeeManager.GetDeptIdArrayAsync(deptId);
            var query = _employeeRepository.GetAll().Where(e => childrenDeptIds.Any(c => e.Department.Contains(c))).Select(e => e.Id);
            return await query.ToArrayAsync();
        }

        /// <summary>
        /// 区县下部门图表
        /// </summary>
        public async Task<ChartDataDto> GetDeptChartDataAsync(long deptIdOrAreaCode, string userId, DateTime? startDate, DateTime? endDate, int tabIndex, string type)
        {
            AreaCodeEnum areaCode = AreaCodeEnum.None;
            if (deptIdOrAreaCode == 1 || deptIdOrAreaCode == 2 || deptIdOrAreaCode == 3)
            {
                areaCode = (AreaCodeEnum)deptIdOrAreaCode;
            }
            //基础查询语句
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll()
                        .WhereIf(startDate.HasValue && tabIndex == 2, s => s.EndTime >= startDate)
                        .WhereIf(endDate.HasValue && tabIndex == 2, s => s.EndTime <= endDate)
                        .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                        on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                        where s.Status == ScheduleMasterStatusEnum.已发布
                        select new
                        {
                            g.EmployeeId,
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status
                        };

            if (areaCode != AreaCodeEnum.None)//当前为区县
            {
                var orgCode = string.Empty;
                switch (areaCode)
                {
                    case AreaCodeEnum.昭化区:
                        {
                            orgCode = GYCode.昭化区;
                        }
                        break;
                    case AreaCodeEnum.剑阁县:
                        {
                            orgCode = GYCode.剑阁县;
                        }
                        break;
                    case AreaCodeEnum.旺苍县:
                        {
                            orgCode = GYCode.旺苍县;
                        }
                        break;
                }
                //部门
                var orgIds = (await _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == orgCode).FirstAsync()).Desc;
                List<ChartDto> dataList = new List<ChartDto>();
                var deptIds = Array.ConvertAll(orgIds.Split(','), o => long.Parse(o));
                var depts = await _organizationRepository.GetAll().Where(o => deptIds.Contains(o.Id)).ToListAsync();
                foreach (var deptId in deptIds)
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptId);
                    var deptName = depts.Where(d => d.Id == deptId).First().DepartmentName;
                    var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));

                    ChartDto chat = new ChartDto();
                    chat.Type = "dept";
                    chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                    chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                    chat.OverdueNum = deptQuery.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                    chat.Id = deptId.ToString();
                    chat.Name = deptName;

                    dataList.Add(chat);
                }

                if (deptIds.Length == 0 || type == "other")//没有部门 或是 其它  直接取其它下面的烟技员
                {
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where e.AreaCode == areaCode
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                       g.Status
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ChartDto()
                                 {
                                     Id = g1.Key.Id,
                                     Name = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                     OverdueNum = g1.Where(g => g.Status == ScheduleStatusEnum.已逾期).Sum(g => g.VisitNum - g.CompleteNum)
                                 });

                    var resultData = new ChartDataDto();
                    resultData.Datas = await rquery.ToListAsync();
                    return resultData;
                }
                else
                {
                    //其它
                    var otherQuery = from g in query
                                     join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                     where e.AreaCode == areaCode
                                     select new
                                     {
                                         g.VisitNum,
                                         g.CompleteNum,
                                         g.Status
                                     };
                    if (otherQuery.Count() > 0)
                    {
                        ChartDto chat = new ChartDto();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.OverdueNum = otherQuery.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                        chat.Id = areaCode.GetHashCode().ToString();
                        chat.Name = "其它";

                        dataList.Add(chat);
                    }
                }
                ChartDataDto result = new ChartDataDto();
                result.Datas = dataList;
                return result;
            }
            else//部门层级
            {
                //子部门
                List<ChartDto> dataList = new List<ChartDto>();
                var depts = await _organizationRepository.GetAll().Where(o => o.ParentId == deptIdOrAreaCode).ToListAsync();
                foreach (var dept in depts)
                {
                    var empIds = await GetEmployeeIdsByDeptId(dept.Id);
                    if (empIds.Length > 0)
                    {
                        var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));
                        if (deptQuery.Count() > 0)
                        {
                            ChartDto chat = new ChartDto();
                            chat.Type = "dept";
                            chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                            chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                            chat.OverdueNum = deptQuery.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                            chat.Id = dept.Id.ToString();
                            chat.Name = dept.DepartmentName;

                            dataList.Add(chat);
                        }
                    }
                }

                if (depts.Count == 0 || type == "other")//没有部门 或是 其它   直接取其它下面的烟技员
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptIdOrAreaCode);
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where empIds.Contains(g.EmployeeId)
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                       g.Status
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ChartDto()
                                 {
                                     Id = g1.Key.Id,
                                     Name = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                     OverdueNum = g1.Where(g => g.Status == ScheduleStatusEnum.已逾期).Sum(g => g.VisitNum - g.CompleteNum)
                                 });

                    var resultData = new ChartDataDto();
                    resultData.Datas = await rquery.ToListAsync();
                    return resultData;
                }
                else
                {
                    var deptstr = "[" + deptIdOrAreaCode + "]";
                    //其它
                    var otherQuery = from g in query
                                     join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                     where e.Department.Contains(deptstr)
                                     select new
                                     {
                                         g.VisitNum,
                                         g.CompleteNum,
                                         g.Status
                                     };
                    if (otherQuery.Count() > 0)
                    {
                        ChartDto chat = new ChartDto();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.OverdueNum = otherQuery.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                        chat.Id = areaCode.GetHashCode().ToString();
                        chat.Name = "其它";

                        dataList.Add(chat);
                    }
                }
                ChartDataDto result = new ChartDataDto();
                result.Datas = dataList;
                return result;
            }
        }

        #endregion

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
                        .WhereIf(endTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
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
            var list = query.OrderBy(v => v.Id).GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
               new SheduleByTaskDto()
               {
                   Id = s.Key.Id,
                   //TaskType=s.Key.Type,
                   TaskName = $"({s.Key.Type}){s.Key.Name}",
                   VisitNum = s.Sum(sd => sd.VisitNum),
                   CompleteNum = s.Sum(sd => sd.CompleteNum),
                   ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
               });
            var result = new ChartByTaskDto();
            result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();

            //分区县统计
            var areaQuery = await (from sd in _scheduleDetailRepository.GetAll()
                                   join s in _scheduleRepository.GetAll()
                                   .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime >= startTime)
                                   .WhereIf(endTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
                                   .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                                   .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                                   on sd.ScheduleId equals s.Id
                                   join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                                   join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                                   where (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)
                                   //&& (g.AreaCode == AreaCodeEnum.昭化区)//添加区县权限 add by donald 2019-1-22
                                   group new { sd.VisitNum, sd.CompleteNum } by new { g.AreaCode } into g
                                   select new ItemDetail
                                   {
                                       VisitNum = g.Sum(v => v.VisitNum.Value),
                                       CompleteNum = g.Sum(v => v.CompleteNum.Value),
                                       AreaCode = g.Key.AreaCode
                                   }).ToListAsync();
            result.AreaItem.AddRange(areaQuery);
            return result;
        }

        /// <summary>
        /// 部门详情
        /// </summary>
        /// <param name="deptIdOrAreaCode"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="tabIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ChartByTaskDto> GetTaskCommChartByGroupAsync(long deptIdOrAreaCode, DateTime? startTime, DateTime? endTime, int tabIndex, string type)
        {
            //AreaCodeEnum areaCode = AreaCodeEnum.None;
            //long areaCode = 0;
            List<TaskChartDto> query = new List<TaskChartDto>();

            var result = new ChartByTaskDto();
            if (deptIdOrAreaCode == 1 || deptIdOrAreaCode == 2 || deptIdOrAreaCode == 3)
            {
                var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                query = await (from sd in _scheduleDetailRepository.GetAll()
                               join s in _scheduleRepository.GetAll()
                               .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime >= startTime)
                               .WhereIf(endTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
                               .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                               .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where g.AreaCode == areaCode
                               select new TaskChartDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
                var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
                 new SheduleByTaskDto()
                 {
                     Id = s.Key.Id,
                     //TaskType=s.Key.Type,
                     TaskName = $"({s.Key.Type}){s.Key.Name}",
                     VisitNum = s.Sum(sd => sd.VisitNum),
                     CompleteNum = s.Sum(sd => sd.CompleteNum),
                     ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                 });
                //result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }
            else if ((deptIdOrAreaCode != 1 && deptIdOrAreaCode != 2 && deptIdOrAreaCode != 3) && type == "other")
            {
                query = await (from sd in _scheduleDetailRepository.GetAll()
                               join s in _scheduleRepository.GetAll()
                               .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime >= startTime)
                               .WhereIf(endTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
                               .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                               .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where e.Department.Contains(deptIdOrAreaCode.ToString())
                               select new TaskChartDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
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
                //result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }
            else
            {
                var empIds = await GetEmployeeIdsByDeptId(deptIdOrAreaCode);
                query = await (from sd in _scheduleDetailRepository.GetAll()
                               join s in _scheduleRepository.GetAll()
                               .WhereIf(startTime.HasValue && tabIndex == 2, s => s.EndTime >= startTime)
                               .WhereIf(endTime.HasValue && tabIndex == 2, s => s.EndTime <= endTime.Value.AddDays(1))
                               .WhereIf(tabIndex == 1, s => s.EndTime >= DateTime.Today)
                               .Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where empIds.Contains(e.Id)
                               select new TaskChartDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
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
                //result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }

            //统计详情
            if (deptIdOrAreaCode == 1 || deptIdOrAreaCode == 2 || deptIdOrAreaCode == 3)//当前为区县
            {
                var orgCode = string.Empty;
                switch ((AreaCodeEnum)deptIdOrAreaCode)
                {
                    case AreaCodeEnum.昭化区:
                        {
                            orgCode = GYCode.昭化区;
                        }
                        break;
                    case AreaCodeEnum.剑阁县:
                        {
                            orgCode = GYCode.剑阁县;
                        }
                        break;
                    case AreaCodeEnum.旺苍县:
                        {
                            orgCode = GYCode.旺苍县;
                        }
                        break;
                }
                //部门
                var orgIds = (await _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == orgCode).FirstAsync()).Desc;
                List<ItemDetail> dataList = new List<ItemDetail>();
                var deptIds = Array.ConvertAll(orgIds.Split(','), o => long.Parse(o));
                var depts = await _organizationRepository.GetAll().Where(o => deptIds.Contains(o.Id)).ToListAsync();
                foreach (var deptId in deptIds)
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptId);
                    var deptName = depts.Where(d => d.Id == deptId).First().DepartmentName;
                    var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));

                    ItemDetail chat = new ItemDetail();
                    chat.Type = "dept";
                    chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                    chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                    chat.DeptId = deptId.ToString();
                    chat.DeptName = deptName;
                    dataList.Add(chat);
                }
                if (deptIds.Length == 0 || type == "other")//没有部门 或是 其它  直接取其它下面的烟技员
                {
                    var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where e.AreaCode == areaCode
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                       g.Status
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ItemDetail()
                                 {
                                     DeptId = g1.Key.Id,
                                     DeptName = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                 });
                    var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s => new SheduleByTaskDto()
                    {
                        Id = s.Key.Id,
                        //TaskType=s.Key.Type,
                        TaskName = s.Key.Name,
                        VisitNum = s.Sum(sd => sd.VisitNum),
                        CompleteNum = s.Sum(sd => sd.CompleteNum),
                        ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                    });
                    //result.AreaItem = await rquery.ToListAsync();
                    result.AreaItem = rquery.ToList();
                    return result;
                }
                else
                {
                    //其它
                    var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                    var otherQuery = (from g in query
                                      join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                      where e.AreaCode == areaCode
                                      select new
                                      {
                                          g.VisitNum,
                                          g.CompleteNum,
                                      }).ToList();
                    if (otherQuery.Count() > 0)
                    {
                        ItemDetail chat = new ItemDetail();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.DeptId = deptIdOrAreaCode.ToString();
                        chat.DeptName = "其它";
                        dataList.Add(chat);
                    }
                }
                result.AreaItem = dataList;
                return result;
            }
            else//部门层级
            {
                //子部门
                List<ItemDetail> dataList = new List<ItemDetail>();
                var depts = await _organizationRepository.GetAll().Where(o => o.ParentId == deptIdOrAreaCode).ToListAsync();
                foreach (var dept in depts)
                {
                    var empIds = await GetEmployeeIdsByDeptId(dept.Id);
                    if (empIds.Length > 0)
                    {
                        var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));
                        if (deptQuery.Count() > 0)
                        {
                            ItemDetail chat = new ItemDetail();
                            chat.Type = "dept";
                            chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                            chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                            chat.DeptId = dept.Id.ToString();
                            chat.DeptName = dept.DepartmentName;
                            dataList.Add(chat);
                        }
                    }
                }

                if (depts.Count == 0 || type == "other")//没有部门 或是 其它   直接取其它下面的烟技员
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptIdOrAreaCode);
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where empIds.Contains(g.EmployeeId)
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ItemDetail()
                                 {
                                     DeptId = g1.Key.Id,
                                     DeptName = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                 });

                    //result.AreaItem = await rquery.ToListAsync();
                    result.AreaItem = rquery.ToList();
                    return result;
                }
                else
                {
                    var deptstr = "[" + deptIdOrAreaCode + "]";
                    //其它
                    var otherQuery = from g in query
                                     join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                     where e.Department.Contains(deptstr)
                                     select new
                                     {
                                         g.VisitNum,
                                         g.CompleteNum,
                                     };
                    if (otherQuery.Count() > 0)
                    {
                        ItemDetail chat = new ItemDetail();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.DeptId = deptIdOrAreaCode.ToString();
                        chat.DeptName = "其它";

                        dataList.Add(chat);
                    }
                }
                result.AreaItem = dataList;
                return result;
            }
            //return result;
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
                //var etnum = query.Where(q => q.Status == ScheduleStatusEnum.已逾期).Sum(q => q.VisitNum - q.CompleteNum);
                //etnum = etnum ?? 0;
                //var etpercent = tnum == 0 ? 0 : Math.Round(etnum.Value / (decimal)tnum.Value, 2); //百分比
                //dataList.Add(new ScheduleSummaryDto() { Num = etnum, Name = "已逾期", ClassName = "overdue", Percent = etpercent * 100, Seq = 3 });

                //待完成数
                //var pnum = tnum - cnum - etnum;
                var pnum = tnum - cnum;
                //var ppercent = tnum == 0 ? 0 : (1M - cpercent - etpercent);
                var ppercent = tnum == 0 ? 0 : (1M - cpercent);
                dataList.Add(new ScheduleSummaryDto() { Num = pnum, Name = "待完成", ClassName = "process", Percent = ppercent * 100, Seq = 2 });

                return dataList.OrderBy(d => d.Seq).ToList();
            });
        }

        /// <summary>
        /// 获取任务的明细信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<SheduleDetailDto>> GetSheduleDetail(int PageIndex, int TabIndex, string DateString, AreaCodeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus, string EmployeeId)
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
                                                   .WhereIf(AreaCode.HasValue && AreaCode != AreaCodeEnum.广元市, g => g.AreaCode == AreaCode)
                                                   .WhereIf(!string.IsNullOrEmpty(EmployeeId), g => g.EmployeeId == EmployeeId)
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
                                                     .WhereIf(AreaCode.HasValue && AreaCode != AreaCodeEnum.广元市, g => g.AreaCode == AreaCode)
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


        /// <summary>
        /// 任务状态查询图标
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public async Task<DistrictChartByStatusDto> GetStatusChartDataAsync(AreaCodeEnum areaCode, int Status)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                         .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                         .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                         .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                        join s in _scheduleRepository.GetAll().Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                        on sd.ScheduleId equals s.Id
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                        where s.EndTime >= DateTime.Today && (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)//添加区县权限
                        select new
                        {
                            sd.VisitNum,
                            sd.CompleteNum,
                            sd.Status,
                            t.Type,
                            t.Name,
                            t.Id
                        };
            var list = query.OrderBy(v => v.Id).GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
               new SheduleByStatusDto()
               {
                   Status = Status,
                   Id = s.Key.Id,
                   TaskName = $"({s.Key.Type}){s.Key.Name}",
                   VisitNum = s.Sum(sd => sd.VisitNum),
                   CompleteNum = s.Sum(sd => sd.CompleteNum - sd.VisitNum > 0 ? sd.VisitNum : sd.CompleteNum),
                   ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
               });
            var result = new DistrictChartByStatusDto();
            result.TasksByStatus = list.OrderBy(l => l.TaskName).ToList();
            //分区县统计
            var areaQuery = await (from sd in _scheduleDetailRepository.GetAll()
                                   .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                         .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                         .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                                   join s in _scheduleRepository.GetAll().Where(s => s.Status == ScheduleMasterStatusEnum.已发布)
                                   on sd.ScheduleId equals s.Id
                                   join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                                   join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                                   where s.EndTime >= DateTime.Today && (areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode)
                                   group new { sd.VisitNum, sd.CompleteNum, sd.Status } by  g.AreaCode into g
                                   select new ItemDetailByStatus
                                   {
                                       Status = Status,
                                       VisitNum = g.Sum(sd => sd.VisitNum),
                                       CompleteNum = g.Sum(sd => sd.CompleteNum - sd.VisitNum > 0 ? sd.VisitNum : sd.CompleteNum),
                                       ExpiredNum = g.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum),
                                       AreaCode = g.Key
                                   }).ToListAsync();
            result.AreaItemByStatus.AddRange(areaQuery);
            return result;
        }

        /// <summary>
        /// 智能报表-当前任务（组织架构）查看
        /// </summary>
        /// <param name="deptIdOrAreaCode"></param>
        /// <param name="type"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public async Task<ChartByStatusDto> GetStatusTaskCommChartAsync(long deptIdOrAreaCode, string type, int Status)
        {
            List<TaskChartByStatusDto> query = new List<TaskChartByStatusDto>();
            var result = new ChartByStatusDto();
            if (deptIdOrAreaCode == 1 || deptIdOrAreaCode == 2 || deptIdOrAreaCode == 3)
            {
                var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                query = await (from sd in _scheduleDetailRepository.GetAll()
                                .WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                         .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                         .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                               join s in _scheduleRepository.GetAll()

                               .Where(s => s.Status == ScheduleMasterStatusEnum.已发布 && s.EndTime >= DateTime.Today)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where areaCode == AreaCodeEnum.广元市 || g.AreaCode == areaCode//添加区县权限
                               select new TaskChartByStatusDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
                var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
                 new SheduleByStatusDto()
                 {
                     Id = s.Key.Id,
                     Status = Status,
                     TaskName = $"({s.Key.Type}){s.Key.Name}",
                     VisitNum = s.Sum(sd => sd.VisitNum),
                     CompleteNum = s.Sum(sd => sd.CompleteNum),
                     ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                 });
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }
            else if ((deptIdOrAreaCode != 1 && deptIdOrAreaCode != 2 && deptIdOrAreaCode != 3) && type == "other")
            {
                query = await (from sd in _scheduleDetailRepository.GetAll().WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                         .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                         .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                               join s in _scheduleRepository.GetAll()
                               .Where(s => s.Status == ScheduleMasterStatusEnum.已发布 && s.EndTime >= DateTime.Today)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where e.Department.Contains(deptIdOrAreaCode.ToString())
                               select new TaskChartByStatusDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
                var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
                 new SheduleByStatusDto()
                 {
                     Id = s.Key.Id,
                     //TaskType=s.Key.Type,
                     Status = Status,
                     TaskName = s.Key.Name,
                     VisitNum = s.Sum(sd => sd.VisitNum),
                     CompleteNum = s.Sum(sd => sd.CompleteNum),
                     ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                 });
                //result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }
            else
            {
                var empIds = await GetEmployeeIdsByDeptId(deptIdOrAreaCode);
                query = await (from sd in _scheduleDetailRepository.GetAll().WhereIf(Status == 2, sd => sd.CompleteNum > 0)
                         .WhereIf(Status == 0, sd => sd.Status == ScheduleStatusEnum.已逾期)
                         .WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                               join s in _scheduleRepository.GetAll().Where(s => s.Status == ScheduleMasterStatusEnum.已发布 && s.EndTime >= DateTime.Today)
                               on sd.ScheduleId equals s.Id
                               join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                               join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                               join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                               where empIds.Contains(e.Id)
                               select new TaskChartByStatusDto
                               {
                                   VisitNum = sd.VisitNum,
                                   CompleteNum = sd.CompleteNum,
                                   Status = sd.Status,
                                   Type = t.Type,
                                   Name = t.Name,
                                   Id = t.Id,
                                   EmployeeId = g.EmployeeId,
                                   Department = e.Department
                               }).ToListAsync();
                var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s =>
                 new SheduleByStatusDto()
                 {
                     Id = s.Key.Id,
                     //TaskType=s.Key.Type,
                     Status = Status,
                     TaskName = s.Key.Name,
                     VisitNum = s.Sum(sd => sd.VisitNum),
                     CompleteNum = s.Sum(sd => sd.CompleteNum),
                     ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                 });
                //result.Tasks = await list.OrderBy(l => l.TaskName).ToListAsync();
                result.Tasks = list.OrderBy(l => l.TaskName).ToList();
            }

            //统计详情
            if (deptIdOrAreaCode == 1 || deptIdOrAreaCode == 2 || deptIdOrAreaCode == 3)//当前为区县
            {
                var orgCode = string.Empty;
                switch ((AreaCodeEnum)deptIdOrAreaCode)
                {
                    case AreaCodeEnum.昭化区:
                        {
                            orgCode = GYCode.昭化区;
                        }
                        break;
                    case AreaCodeEnum.剑阁县:
                        {
                            orgCode = GYCode.剑阁县;
                        }
                        break;
                    case AreaCodeEnum.旺苍县:
                        {
                            orgCode = GYCode.旺苍县;
                        }
                        break;
                }
                //部门
                var orgIds = (await _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == orgCode).FirstAsync()).Desc;
                List<ItemDetailByStatus> dataList = new List<ItemDetailByStatus>();
                var deptIds = Array.ConvertAll(orgIds.Split(','), o => long.Parse(o));
                var depts = await _organizationRepository.GetAll().Where(o => deptIds.Contains(o.Id)).ToListAsync();
                foreach (var deptId in deptIds)
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptId);
                    var deptName = depts.Where(d => d.Id == deptId).First().DepartmentName;
                    var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));

                    ItemDetailByStatus chat = new ItemDetailByStatus();
                    chat.Type = "dept";
                    chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                    chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                    chat.ExpiredNum = deptQuery.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum);
                    chat.Status = Status;
                    chat.DeptId = deptId.ToString();
                    chat.DeptName = deptName;
                    dataList.Add(chat);
                }
                if (deptIds.Length == 0 || type == "other")//没有部门 或是 其它  直接取其它下面的烟技员
                {
                    var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where e.AreaCode == areaCode
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                       g.Status
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ItemDetailByStatus()
                                 {
                                     DeptId = g1.Key.Id,
                                     DeptName = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                     Status = Status,
                                     ExpiredNum = g1.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                                 });
                    var list = query.GroupBy(s => new { s.Name, s.Type, s.Id }).Select(s => new SheduleByStatusDto()
                    {
                        Id = s.Key.Id,
                        //TaskType=s.Key.Type,
                          Status = Status,
                        TaskName = s.Key.Name,
                        VisitNum = s.Sum(sd => sd.VisitNum),
                        CompleteNum = s.Sum(sd => sd.CompleteNum),
                        ExpiredNum = s.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                    });
                    //result.AreaItem = await rquery.ToListAsync();
                    result.AreaItem = rquery.ToList();
                    return result;
                }
                else
                {
                    //其它
                    var areaCode = (AreaCodeEnum)deptIdOrAreaCode;
                    var otherQuery = (from g in query
                                      join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                      where e.AreaCode == areaCode
                                      select new
                                      {
                                          g.VisitNum,
                                          g.CompleteNum,
                                          g.Status
                                      }).ToList();
                    if (otherQuery.Count() > 0)
                    {
                        ItemDetailByStatus chat = new ItemDetailByStatus();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.ExpiredNum = otherQuery.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum);
                        chat.Status = Status;
                        chat.DeptId = deptIdOrAreaCode.ToString();
                        chat.DeptName = "其它";
                        dataList.Add(chat);
                    }
                }
                result.AreaItem = dataList;
                return result;
            }
            else//部门层级
            {
                //子部门
                List<ItemDetailByStatus> dataList = new List<ItemDetailByStatus>();
                var depts = await _organizationRepository.GetAll().Where(o => o.ParentId == deptIdOrAreaCode).ToListAsync();
                foreach (var dept in depts)
                {
                    var empIds = await GetEmployeeIdsByDeptId(dept.Id);
                    if (empIds.Length > 0)
                    {
                        var deptQuery = query.Where(q => empIds.Contains(q.EmployeeId));
                        if (deptQuery.Count() > 0)
                        {
                            ItemDetailByStatus chat = new ItemDetailByStatus();
                            chat.Type = "dept";
                            chat.VisitNum = deptQuery.Select(q => q.VisitNum).Sum();
                            chat.CompleteNum = deptQuery.Select(q => q.CompleteNum).Sum();
                            chat.ExpiredNum = deptQuery.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum);
                            chat.Status = Status;
                            chat.DeptId = dept.Id.ToString();
                            chat.DeptName = dept.DepartmentName;
                            dataList.Add(chat);
                        }
                    }
                }

                if (depts.Count == 0 || type == "other")//没有部门 或是 其它   直接取其它下面的烟技员
                {
                    var empIds = await GetEmployeeIdsByDeptId(deptIdOrAreaCode);
                    var empQuery = from g in query
                                   join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                   where empIds.Contains(g.EmployeeId)
                                   select new
                                   {
                                       e.Id,
                                       e.Name,
                                       g.VisitNum,
                                       g.CompleteNum,
                                       g.Status
                                   };
                    var rquery = empQuery.GroupBy(g => new { g.Id, g.Name }).Select(g1 =>
                                 new ItemDetailByStatus()
                                 {
                                     DeptId = g1.Key.Id,
                                     DeptName = g1.Key.Name,
                                     Type = "employee",
                                     VisitNum = g1.Sum(g => g.VisitNum),
                                     CompleteNum = g1.Sum(g => g.CompleteNum),
                                     Status = Status,
                                     ExpiredNum = g1.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum)
                                 });

                    //result.AreaItem = await rquery.ToListAsync();
                    result.AreaItem = rquery.ToList();
                    return result;
                }
                else
                {
                    var deptstr = "[" + deptIdOrAreaCode + "]";
                    //其它
                    var otherQuery = from g in query
                                     join e in _employeeRepository.GetAll() on g.EmployeeId equals e.Id
                                     where e.Department.Contains(deptstr)
                                     select new
                                     {
                                         g.VisitNum,
                                         g.CompleteNum,
                                         g.Status
                                     };
                    if (otherQuery.Count() > 0)
                    {
                        ItemDetailByStatus chat = new ItemDetailByStatus();
                        chat.Type = "other";
                        chat.VisitNum = otherQuery.Select(q => q.VisitNum).Sum();
                        chat.CompleteNum = otherQuery.Select(q => q.CompleteNum).Sum();
                        chat.ExpiredNum = otherQuery.Where(sd => sd.Status == ScheduleStatusEnum.已逾期).Sum(sd => sd.VisitNum - sd.CompleteNum);
                        chat.Status = Status;
                        chat.DeptId = deptIdOrAreaCode.ToString();
                        chat.DeptName = "其它";

                        dataList.Add(chat);
                    }
                }
                result.AreaItem = dataList;
                return result;
            }
            //return result;
        }
    }
}
