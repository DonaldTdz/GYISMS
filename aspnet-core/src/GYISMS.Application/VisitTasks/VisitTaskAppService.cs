
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

using GYISMS.VisitTasks.Authorization;
using GYISMS.VisitTasks.Dtos;
using GYISMS.VisitTasks;
using GYISMS.Authorization;
using GYISMS.TaskExamines;
using GYISMS.TaskExamines.Dtos;
using GYISMS.ScheduleTasks;

namespace GYISMS.VisitTasks
{
    /// <summary>
    /// VisitTask应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class VisitTaskAppService : GYISMSAppServiceBase, IVisitTaskAppService
    {
        private readonly IRepository<VisitTask, int> _visittaskRepository;
        private readonly IVisitTaskManager _visittaskManager;
        private readonly IRepository<TaskExamine, int> _taskexamineRepository;
        private readonly IRepository<ScheduleTask, Guid> _scheduletaskRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public VisitTaskAppService(IRepository<VisitTask, int> visittaskRepository
            , IVisitTaskManager visittaskManager
            , IRepository<TaskExamine, int> taskexamineRepository
            , IRepository<ScheduleTask, Guid> scheduletaskRepository
            )
        {
            _visittaskRepository = visittaskRepository;
            _visittaskManager = visittaskManager;
            _taskexamineRepository = taskexamineRepository;
            _scheduletaskRepository = scheduletaskRepository;
        }


        /// <summary>
        /// 获取VisitTask的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VisitTaskListDto>> GetPagedVisitTasksAsync(GetVisitTasksInput input)
        {
            var query = _visittaskRepository.GetAll().Where(v => v.IsDeleted == false)
                     .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                     .WhereIf(input.TaskType.HasValue, r => r.Type == input.TaskType);
            // TODO:根据传入的参数添加过滤条件

            var visittaskCount = await query.CountAsync();

            var visittasks = await query
                    .OrderBy(v => v.Type).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var visittaskListDtos = ObjectMapper.Map<List <VisitTaskListDto>>(visittasks);
            var visittaskListDtos = visittasks.MapTo<List<VisitTaskListDto>>();

            return new PagedResultDto<VisitTaskListDto>(
                    visittaskCount,
                    visittaskListDtos
                );
        }

        /// <summary>
        /// 新增任务查询Checbox状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<VisitTaskListDto>> GetVisitTasksWithStatusAsync(GetVisitTasksInput input)
        {
            var visitList = _visittaskRepository.GetAll().Where(v => v.IsDeleted == false);
            //var taskList = _scheduletaskRepository.GetAll().Where(v =>v.IsDeleted ==false&& v.ScheduleId == input.ScheduleId);
            var taskList = _scheduletaskRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId);
            var query = await (from v in visitList
                               select new VisitTaskListDto
                               {
                                   Id = v.Id,
                                   Name = v.Name,
                                   Type = v.Type,
                               }).AsNoTracking().ToListAsync();
            var taskDto = from t in taskList
                          select new
                          {
                              t.TaskId,
                              t.VisitNum,
                              t.Id,
                              t.IsDeleted
                          };
            foreach (var taskItem in taskDto)
            {
                foreach (var item in query)
                {
                    if (item.Id == taskItem.TaskId)
                    {
                        if (taskItem.IsDeleted == true)
                        {
                            item.Checked = false;
                            item.VisitNum = 1;
                        }
                        else
                        {
                            item.Checked = true;
                            item.VisitNum = taskItem.VisitNum;
                        }
                        item.ScheduleTaskId = taskItem.Id;
                        break;
                    }
                }
            }
            return query.OrderByDescending(v => v.Checked).ThenBy(v => v.TypeName).ToList();
        }

        /// <summary>
        /// 获取拜访任务列表不分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<VisitTaskListDto>> GetVisitTasksListAsync(GetVisitTasksInput input)
        {
            var query = _visittaskRepository.GetAll().Where(v => v.IsDeleted == false)
                     .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name));
            var visittasks = await query.OrderBy(v => v.Type).AsNoTracking().ToListAsync();
            var visittaskListDtos = visittasks.MapTo<List<VisitTaskListDto>>();
            return visittaskListDtos;
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetVisitTaskForEditOutput> GetVisitTaskForEdit(NullableIdDto<int> input)
        {
            var output = new GetVisitTaskForEditOutput();
            VisitTaskEditDto visittaskEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _visittaskRepository.GetAsync(input.Id.Value);

                visittaskEditDto = entity.MapTo<VisitTaskEditDto>();

                //visittaskEditDto = ObjectMapper.Map<List <visittaskEditDto>>(entity);
            }
            else
            {
                visittaskEditDto = new VisitTaskEditDto();
            }

            output.VisitTask = visittaskEditDto;
            return output;
        }


        /// <summary>
        /// 新增VisitTask
        /// </summary>
        protected virtual async Task<VisitTaskEditDto> CreateVisitTaskAsync(VisitTaskEditDto input)
        {
            var entity = ObjectMapper.Map<VisitTask>(input);
            entity.IsDeleted = false;
            var id = await _visittaskRepository.InsertAndGetIdAsync(entity);
            VisitTaskEditDto list = new VisitTaskEditDto();
            list.Id = entity.Id;
            list.Name = entity.Name;
            list.Type = entity.Type;
            list.IsDeleted = entity.IsDeleted;
            list.IsExamine = entity.IsExamine;
            list.Desc = entity.Desc;
            list.TaskExamineList = new List<TaskExamineEditDto>();
            await CurrentUnitOfWork.SaveChangesAsync();
            if (entity.IsExamine == true)
            {
                foreach (var item in input.TaskExamineList)
                {
                    item.TaskId = id;
                    var examEntity = ObjectMapper.Map<TaskExamine>(item);
                    examEntity.IsDeleted = false;
                    await _taskexamineRepository.InsertAndGetIdAsync(examEntity);
                    var temp = examEntity.MapTo<TaskExamineEditDto>();
                    list.TaskExamineList.Add(temp);
                }
            }
            return list;
        }

        /// <summary>
        /// 编辑VisitTask
        /// </summary>
        protected virtual async Task<VisitTaskEditDto> UpdateVisitTaskAsync(VisitTaskEditDto input)
        {
            VisitTaskEditDto list = new VisitTaskEditDto();
            var entity = await _visittaskRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            var result = await _visittaskRepository.UpdateAsync(entity);
            list.Id = result.Id;
            list.Name = result.Name;
            list.Type = result.Type;
            list.IsDeleted = result.IsDeleted;
            list.IsExamine = result.IsExamine;
            list.Desc = result.Desc;
            list.TaskExamineList = new List<TaskExamineEditDto>();
            await CurrentUnitOfWork.SaveChangesAsync();

            //var temp = result.MapTo<VisitTaskEditDto>();
            if (entity.IsExamine == true)
            {
                foreach (var item in input.TaskExamineList)
                {
                    if (item.Id != 0)
                    {
                        var temp = await UpdateTaskExamineAsync(item);
                        list.TaskExamineList.Add(temp);
                    }
                    else
                    {
                        item.TaskId = result.Id;
                        var temp = await CreateTaskExamineAsync(item);
                        list.TaskExamineList.Add(temp);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 添加或者修改TaskExamine的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateTaskExamineAsync(TaskExamineEditDto input)
        {

            if (input.Id == 0)
            {
                await UpdateTaskExamineAsync(input);
            }
            else
            {
                await CreateTaskExamineAsync(input);
            }
        }

        /// <summary>
        /// 新增TaskExamine
        /// </summary>
        protected virtual async Task<TaskExamineEditDto> CreateTaskExamineAsync(TaskExamineEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<TaskExamine>(input);
            entity.IsDeleted = false;
            var id = await _taskexamineRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<TaskExamineEditDto>();
        }

        /// <summary>
        /// 编辑TaskExamine
        /// </summary>
        protected virtual async Task<TaskExamineEditDto> UpdateTaskExamineAsync(TaskExamineEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _taskexamineRepository.GetAsync(input.Id);
            input.MapTo(entity);
            var result = await _taskexamineRepository.UpdateAsync(entity);
            return entity.MapTo<TaskExamineEditDto>();
        }

        /// <summary>
        /// 删除VisitTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VisitTaskAppPermissions.VisitTask_Delete)]
        public async Task DeleteVisitTask(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _visittaskRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除VisitTask的方法
        /// </summary>
        [AbpAuthorize(VisitTaskAppPermissions.VisitTask_BatchDelete)]
        public async Task BatchDeleteVisitTasksAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _visittaskRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增或修改任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<VisitTaskEditDto> CreateOrUpdateVisitTaskAsync(VisitTaskEditDto input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateVisitTaskAsync(input);
            }
            else
            {
                return await CreateVisitTaskAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VisitTaskListDto> GetVisitTaskByIdAsync(int id)
        {
            var entity = await _visittaskRepository.GetAsync(id);
            return entity.MapTo<VisitTaskListDto>();
        }

        /// <summary>
        /// 删除任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task VisitTaskDeleteByIdAsync(VisitTaskEditDto input)
        {
            var entity = await _visittaskRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _visittaskRepository.UpdateAsync(entity);
        }
    }
}