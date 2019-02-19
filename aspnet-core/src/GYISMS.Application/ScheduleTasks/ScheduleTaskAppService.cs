
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

using GYISMS.ScheduleTasks.Authorization;
using GYISMS.ScheduleTasks.Dtos;
using GYISMS.ScheduleTasks;
using GYISMS.Authorization;
using GYISMS.Schedules;
using GYISMS.ScheduleDetails;
using GYISMS.VisitTasks;
using GYISMS.GYEnums;
using GYISMS.Growers;
using GYISMS.TaskExamines;
using GYISMS.VisitRecords;
using GYISMS.Growers.Dtos;
using Abp.Auditing;
using GYISMS.SystemDatas;
using GYISMS.GrowerLocationLogs;
using GYISMS.GrowerAreaRecords;

namespace GYISMS.ScheduleTasks
{
    /// <summary>
    /// ScheduleTask应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]

    public class ScheduleTaskAppService : GYISMSAppServiceBase, IScheduleTaskAppService
    {
        private readonly IRepository<ScheduleTask, Guid> _scheduletaskRepository;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly IRepository<ScheduleDetail, Guid> _scheduleDetailRepository;
        private readonly IRepository<VisitTask> _visitTaskRepository;
        private readonly IRepository<Grower> _growerRepository;
        private readonly IRepository<VisitRecord, Guid> _visitRecordRepository;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly IRepository<GrowerLocationLog, Guid> _growerLocationLogRepository;
        private readonly IRepository<GrowerAreaRecord, Guid> _growerAreaRecordRepository;
        private readonly IScheduleTaskManager _scheduletaskManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleTaskAppService(IRepository<ScheduleTask, Guid> scheduletaskRepository
            , IScheduleTaskManager scheduletaskManager
            , IRepository<Schedule, Guid> scheduleRepository
            , IRepository<VisitTask> visitTaskRepository
            , IRepository<ScheduleDetail, Guid> scheduleDetailRepository
            , IRepository<Grower> growerRepository
            , IRepository<VisitRecord, Guid> visitRecordRepository
            , IRepository<SystemData, int> systemdataRepository
            , IRepository<GrowerLocationLog, Guid> growerLocationLogRepository
            , IRepository<GrowerAreaRecord, Guid> growerAreaRecordRepository
            )
        {
            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _scheduletaskManager = scheduletaskManager;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
            _visitRecordRepository = visitRecordRepository;
            _systemdataRepository = systemdataRepository;
            _growerLocationLogRepository = growerLocationLogRepository;
            _growerAreaRecordRepository = growerAreaRecordRepository;
        }


        /// <summary>
        /// 获取ScheduleTask的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ScheduleTaskListDto>> GetPagedScheduleTasksAsync(GetScheduleTasksInput input)
        {

            var query = _scheduletaskRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var scheduletaskCount = await query.CountAsync();

            var scheduletasks = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var scheduletaskListDtos = ObjectMapper.Map<List <ScheduleTaskListDto>>(scheduletasks);
            var scheduletaskListDtos = scheduletasks.MapTo<List<ScheduleTaskListDto>>();

            return new PagedResultDto<ScheduleTaskListDto>(
                        scheduletaskCount,
                        scheduletaskListDtos
                );
        }


        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetScheduleTaskForEditOutput> GetScheduleTaskForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetScheduleTaskForEditOutput();
            ScheduleTaskEditDto scheduletaskEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _scheduletaskRepository.GetAsync(input.Id.Value);

                scheduletaskEditDto = entity.MapTo<ScheduleTaskEditDto>();

                //scheduletaskEditDto = ObjectMapper.Map<List <scheduletaskEditDto>>(entity);
            }
            else
            {
                scheduletaskEditDto = new ScheduleTaskEditDto();
            }

            output.ScheduleTask = scheduletaskEditDto;
            return output;
        }


        /// <summary>
        /// 新增ScheduleTask
        /// </summary>
        protected virtual async Task<ScheduleTaskEditDto> CreateScheduleTaskAsync(ScheduleTaskEditDto input)
        {
            var entity = ObjectMapper.Map<ScheduleTask>(input);
            entity.IsDeleted = false;
            //entity.CreationTime = DateTime.Now;
            entity = await _scheduletaskRepository.InsertAsync(entity);
            return entity.MapTo<ScheduleTaskEditDto>();
        }

        /// <summary>
        /// 编辑ScheduleTask
        /// </summary>
        protected virtual async Task<ScheduleTaskEditDto> UpdateScheduleTaskAsync(ScheduleTaskEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _scheduletaskRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = false;
            // ObjectMapper.Map(input, entity);
            var result = await _scheduletaskRepository.UpdateAsync(entity);
            return result.MapTo<ScheduleTaskEditDto>();
        }



        /// <summary>
        /// 删除ScheduleTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ScheduleTaskAppPermissions.ScheduleTask_Delete)]
        public async Task DeleteScheduleTask(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _scheduletaskRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ScheduleTask的方法
        /// </summary>
        public async Task BatchDeleteScheduleTasksAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _scheduletaskRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增或修改计划任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ScheduleTaskEditDto>> CreateOrUpdateScheduleTaskAsync(List<ScheduleTaskEditDto> input)
        {
            List<ScheduleTaskEditDto> list = new List<ScheduleTaskEditDto>();
            foreach (var item in input)
            {
                if (item.Id.HasValue)
                {
                    list.Add(await UpdateScheduleTaskAsync(item));
                }
                else
                {
                    list.Add(await CreateScheduleTaskAsync(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 根据id获取计划任务信息
        /// </summary>
        public async Task<ScheduleTaskListDto> GetScheduleTaskByIdAsync(Guid id)
        {
            var entity = await _scheduletaskRepository.GetAsync(id);
            return entity.MapTo<ScheduleTaskListDto>();
        }

        /// <summary>
        /// 计划任务列表不分页
        /// </summary>
        public async Task<List<ScheduleTaskListDto>> GetScheduleTasksNoPageAsync(Guid id)
        {
            var scheduleTask = _scheduletaskRepository.GetAll().Where(v => v.ScheduleId == id && v.IsDeleted == false);
            var visitTask = _visitTaskRepository.GetAll();
            var query = await (from st in scheduleTask
                               join vt in visitTask on st.TaskId equals vt.Id
                               select new ScheduleTaskListDto()
                               {
                                   Id = st.Id,
                                   TaskName = st.TaskName,
                                   IsExamine = vt.IsExamine,
                                   TypeName = vt.Type.ToString(),
                                   TaskId = vt.Id,
                                   ScheduleId = st.ScheduleId,
                                   VisitNum = st.VisitNum,
                                   IsDeleted = st.IsDeleted,
                                   CreationTime = st.CreationTime,
                                   CreatorUserId = st.CreatorUserId,
                                   LastModificationTime = st.LastModificationTime,
                                   LastModifierUserId = st.LastModifierUserId,
                               }).OrderByDescending(v => v.TypeName).ThenByDescending(v => v.IsExamine).AsNoTracking().ToListAsync();
            return query;
        }

        /// <summary>
        /// 删除指派任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task VisitTaskDeleteByIdAsync(ScheduleTaskEditDto input)
        {
            var entity = await _scheduletaskRepository.GetAsync(input.Id.Value);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _scheduletaskRepository.UpdateAsync(entity);
            List<Guid> detailIds = await _scheduleDetailRepository.GetAll().Where(v => v.ScheduleTaskId == input.Id).AsNoTracking().Select(v => v.Id).ToListAsync();
            await BatchDeleteScheduleDetailsAsync(detailIds);
        }

        /// <summary>
        /// 批量删除任务指派信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDeleteScheduleDetailsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _scheduleDetailRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        #region 钉钉客户端

        /// <summary>
        /// 获取钉钉用户任务列表
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<List<DingDingScheduleTaskDto>> GetDingDingScheduleTaskListAsync(string userId)
        {
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
                            st.Id,
                            st.ScheduleId,
                            t.Type,
                            t.Name,
                            sd.VisitNum,
                            sd.CompleteNum
                        };

            var taskList = from ts in (from q in query
                                       group q by new { q.Id, q.ScheduleId, q.Type, q.Name } into qg
                                       select new
                                       {
                                           qg.Key.Id,
                                           qg.Key.ScheduleId,
                                           TaskName = qg.Key.Name,
                                           TaskType = qg.Key.Type,
                                           NumTotal = qg.Sum(q => q.VisitNum),
                                           CompleteNum = qg.Sum(q => q.CompleteNum)
                                       })
                           join s in _scheduleRepository.GetAll()
                           on ts.ScheduleId equals s.Id
                           select new DingDingScheduleTaskDto()
                           {
                               Id = ts.Id,
                               TaskName = ts.TaskName,
                               TaskType = ts.TaskType,
                               CompleteNum = ts.CompleteNum,
                               NumTotal = ts.NumTotal,
                               EndTime = s.EndTime
                           };

            var dataList = await taskList.ToListAsync();
            return dataList.OrderBy(d => d.EndDay).ToList();
        }

        /// <summary>
        ///获取任务详情
        ///<param name="Status">(2表示全部完成、3表示待完成)</param>
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<DingDingTaskDto> GetDingDingTaskInfoAsync(Guid scheduleTaskId, string uid, int Status)
        {
            //基本信息
            var query = from st in _scheduletaskRepository.GetAll()
                        join s in _scheduleRepository.GetAll() on st.ScheduleId equals s.Id
                        join t in _visitTaskRepository.GetAll() on st.TaskId equals t.Id
                        where st.Id == scheduleTaskId
                        select new DingDingTaskDto()
                        {
                            Id = st.Id,
                            BeginTime = s.BeginTime,
                            EndTime = s.EndTime,
                            ScheduleTitle = s.Name,
                            TaskNam = st.TaskName,
                            TaskType = t.Type
                        };
            var taskDto = await query.FirstOrDefaultAsync();

            //烟农信息
            var growerQuery = from sd in _scheduleDetailRepository.GetAll()
                                  //.WhereIf(Status == 2, sd => sd.CompleteNum == sd.VisitNum)
                                  //.WhereIf(Status == 3, sd => sd.Status != ScheduleStatusEnum.已逾期 && sd.CompleteNum < sd.VisitNum)
                              join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                              where sd.ScheduleTaskId == scheduleTaskId
                              && sd.EmployeeId == uid
                              select new DingDingTaskGrowerDto()
                              {
                                  Id = sd.Id,
                                  CompleteNum = sd.CompleteNum,
                                  GrowerName = sd.GrowerName,
                                  VisitNum = sd.VisitNum,
                                  UnitName = g.UnitName,
                                  Status = sd.Status,
                                  CreationTime = sd.CreationTime
                              };
            taskDto.Growers = await growerQuery.OrderByDescending(s => s.CreationTime).ToListAsync();

            taskDto.VisitTotal = taskDto.Growers.Sum(g => g.VisitNum).Value;
            taskDto.CompleteNum = taskDto.Growers.Sum(g => g.CompleteNum).Value;

            return taskDto;
        }

        [AbpAllowAnonymous]
        [Audited]
        public async Task<DingDingVisitGrowerDetailDto> GetDingDingVisitGrowerDetailAsync(Guid scheduleDetailId)
        {
            //详情
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        join s in _scheduleRepository.GetAll() on sd.ScheduleId equals s.Id
                        where sd.Id == scheduleDetailId
                        select new DingDingVisitGrowerDetailDto()
                        {
                            Id = sd.Id,
                            ScheduleName = s.Name,
                            TaskNam = t.Name,
                            TaskType = t.Type,
                            GrowerId = sd.GrowerId,
                            VisitNum = sd.VisitNum,
                            CompleteNum = sd.CompleteNum,
                            ScheduleStatus = sd.Status,
                            BeginTime = s.BeginTime,
                            EndTime = s.EndTime
                        };

            var taskDetailDto = await query.FirstOrDefaultAsync();
            //获取烟农位置修改次数限制
            var systemData = await _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.LocationLimitCode).FirstOrDefaultAsync();
            var limitNum = 3;
            if (systemData != null && !string.IsNullOrEmpty(systemData.Desc))
            {
                limitNum = int.Parse(systemData.Desc);
            }
            var date = DateTime.Now;
            var startTime = DateTime.Parse(date.Year + "-1-1");
            var endTime = DateTime.Parse((date.Year + 1) + "-1-1");
            var num = await _growerLocationLogRepository.GetAll().Where(g => g.CreationTime >= startTime && g.CreationTime < endTime && g.GrowerId == taskDetailDto.GrowerId.Value).CountAsync();
            taskDetailDto.GrowerInfo = (await _growerRepository.GetAsync(taskDetailDto.GrowerId.Value)).MapTo<GrowerListDto>();
            taskDetailDto.GrowerInfo.LimitNum = limitNum;
            taskDetailDto.GrowerInfo.CollectNum = num;
            if (taskDetailDto.TaskType == TaskTypeEnum.面积落实)//新增面积落实 2019-2-18
            {
                taskDetailDto.VisitRecords = await _growerAreaRecordRepository.GetAll()
                                                    .Where(g => g.ScheduleDetailId == scheduleDetailId && g.GrowerId == taskDetailDto.GrowerId)
                                                    .Select(g => new DingDingVisitRecordDto()
                                                    {
                                                        Id = g.Id,
                                                        CreationTime = g.CollectionTime,
                                                        Desc = g.Remark,
                                                        ImgPath = g.ImgPath,
                                                        Location = g.Location,
                                                        SignTime = g.CollectionTime,
                                                        Area = g.Area
                                                    }).ToListAsync();
            }
            else
            {
                taskDetailDto.VisitRecords = (await _visitRecordRepository.GetAll()
                                              .Where(v => v.ScheduleDetailId == scheduleDetailId)
                                              .OrderBy(v => v.CreationTime)
                                              .ToListAsync()).MapTo<List<DingDingVisitRecordDto>>();
            }

            return taskDetailDto;
        }


        [AbpAllowAnonymous]
        [Audited]
        public async Task<DingDingVisitGrowerDetailDto> GetDingDingGrowerDetailAsync(int id)
        {
            //详情
            var taskDetailDto = new DingDingVisitGrowerDetailDto();
            taskDetailDto.GrowerId = id;
            taskDetailDto.GrowerInfo = (await _growerRepository.GetAsync(id)).MapTo<GrowerListDto>();
            taskDetailDto.VisitRecords = await _growerAreaRecordRepository.GetAll()
                                                .Where(g => g.ScheduleDetailId == taskDetailDto.GrowerInfo.AreaScheduleDetailId && g.GrowerId == id)
                                                .Select(g => new DingDingVisitRecordDto()
                                                {
                                                    Id = g.Id,
                                                    CreationTime = g.CollectionTime,
                                                    Desc = g.Remark,
                                                    ImgPath = g.ImgPath,
                                                    Location = g.Location,
                                                    SignTime = g.CollectionTime,
                                                    Area = g.Area
                                                }).ToListAsync();
            return taskDetailDto;
        }

        [AbpAllowAnonymous]
        [Audited]
        public async Task<List<DingDingScheduleDetailDto>> GetDingDingScheduleTaskPagingAsync(string userId, ScheduleStatusEnum status, DateTime? startDate, DateTime? endDate, int pageIndex)
        {
            var query = from sd in _scheduleDetailRepository.GetAll().WhereIf(status != ScheduleStatusEnum.None, s => s.Status == status)
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        join s in _scheduleRepository.GetAll()
                                        .WhereIf(startDate.HasValue, s => s.EndTime >= startDate)
                                        .WhereIf(endDate.HasValue, s => s.EndTime <= endDate)
                        on sd.ScheduleId equals s.Id
                        where sd.EmployeeId == userId
                        && s.Status == ScheduleMasterStatusEnum.已发布
                        select new DingDingScheduleDetailDto()
                        {
                            Id = sd.Id,
                            EndTime = s.EndTime,
                            GrowerId = sd.GrowerId,
                            GrowerName = sd.GrowerName,
                            Status = sd.Status,
                            TaskName = t.Name,
                            TaskType = t.Type
                        };

            var dataList = await query.OrderByDescending(q => q.EndTime).Skip(pageIndex).Take(15).ToListAsync();
            return dataList;
        }

        #endregion


    }
}


