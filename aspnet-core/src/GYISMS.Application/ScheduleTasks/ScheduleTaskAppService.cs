
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
            )
        {
            _scheduletaskRepository = scheduletaskRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _scheduletaskManager = scheduletaskManager;
            _visitTaskRepository = visitTaskRepository;
            _growerRepository = growerRepository;
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
            //TODO:新增前的逻辑判断，是否允许新增
            if (input.VisitNum == null)
            {
                input.VisitNum = 5;
            }
            var entity = ObjectMapper.Map<ScheduleTask>(input);
            entity.CreationTime = DateTime.Now;
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
        [AbpAuthorize(ScheduleTaskAppPermissions.ScheduleTask_BatchDelete)]
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
        public async Task<ScheduleTaskEditDto> CreateOrUpdateScheduleTaskAsycn(ScheduleTaskEditDto input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateScheduleTaskAsync(input);
            }
            else
            {
                return await CreateScheduleTaskAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取计划任务信息
        /// </summary>
        public async Task<ScheduleTaskListDto> GetScheduleTaskByIdAsync(Guid id)
        {
            var entity = await _scheduletaskRepository.GetAsync(id);
            return entity.MapTo<ScheduleTaskListDto>();
        }

        #region 钉钉客户端

        /// <summary>
        /// 获取钉钉用户任务列表
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<List<DingDingScheduleTaskDto>> GetDingDingScheduleTaskListAsync(string userId)
        {
            var query = from st in _scheduletaskRepository.GetAll()
                        join sd in _scheduleDetailRepository.GetAll() on st.Id equals sd.ScheduleTaskId
                        join t in _visitTaskRepository.GetAll() on st.TaskId equals t.Id
                        join s in _scheduleRepository.GetAll() on st.ScheduleId equals s.Id
                        where sd.EmployeeId == userId
                        && (sd.Status == ScheduleStatusEnum.未开始 || sd.Status == ScheduleStatusEnum.进行中)
                        && s.Status == ScheduleMasterStatusEnum.已发布
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
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<DingDingTaskDto> GetDingDingTaskInfoAsync(Guid scheduleTaskId)
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
                              join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id
                              where sd.ScheduleTaskId == scheduleTaskId
                              select new DingDingTaskGrowerDto()
                              {
                                  Id = sd.Id,
                                  CompleteNum = sd.CompleteNum,
                                  GrowerName = sd.GrowerName,
                                  VisitNum = sd.VisitNum,
                                  UnitName = g.UnitName
                              };
            taskDto.Growers = await growerQuery.ToListAsync();

            taskDto.VisitTotal = taskDto.Growers.Sum(g => g.VisitNum).Value;
            taskDto.CompleteNum = taskDto.Growers.Sum(g => g.CompleteNum).Value;
            
            return taskDto;
        }

        public Task<DingDingVisitGrowerDetailDto> GetDingDingVisitGrowerDetailAsync(Guid scheduleTaskId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}


