
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

using GYISMS.TaskExamines.Authorization;
using GYISMS.TaskExamines.Dtos;
using GYISMS.TaskExamines;
using GYISMS.Authorization;

namespace GYISMS.TaskExamines
{
    /// <summary>
    /// TaskExamine应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class TaskExamineAppService : GYISMSAppServiceBase, ITaskExamineAppService
    {
        private readonly IRepository<TaskExamine, int> _taskexamineRepository;


        private readonly ITaskExamineManager _taskexamineManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public TaskExamineAppService(IRepository<TaskExamine, int> taskexamineRepository
            , ITaskExamineManager taskexamineManager
            )
        {
            _taskexamineRepository = taskexamineRepository;
            _taskexamineManager = taskexamineManager;
        }


        /// <summary>
        /// 获取TaskExamine的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TaskExamineListDto>> GetPagedTaskExaminesAsync(GetTaskExaminesInput input)
        {

            var query = _taskexamineRepository.GetAll().Where(v=>v.IsDeleted ==false && v.TaskId ==input.TaskId);
            // TODO:根据传入的参数添加过滤条件

            var taskexamineCount = await query.CountAsync();

            var taskexamines = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var taskexamineListDtos = ObjectMapper.Map<List <TaskExamineListDto>>(taskexamines);
            var taskexamineListDtos = taskexamines.MapTo<List<TaskExamineListDto>>();

            return new PagedResultDto<TaskExamineListDto>(
                    taskexamineCount,
                    taskexamineListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取TaskExamineListDto信息
        /// </summary>
        public async Task<TaskExamineListDto> GetTaskExamineByIdAsync(int id)
        {
            var entity = await _taskexamineRepository.GetAll().Where(v => v.TaskId == id).FirstOrDefaultAsync();
            return entity.MapTo<TaskExamineListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetTaskExamineForEditOutput> GetTaskExamineForEdit(NullableIdDto<int> input)
        {
            var output = new GetTaskExamineForEditOutput();
            TaskExamineEditDto taskexamineEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _taskexamineRepository.GetAsync(input.Id.Value);

                taskexamineEditDto = entity.MapTo<TaskExamineEditDto>();

                //taskexamineEditDto = ObjectMapper.Map<List <taskexamineEditDto>>(entity);
            }
            else
            {
                taskexamineEditDto = new TaskExamineEditDto();
            }

            output.TaskExamine = taskexamineEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改TaskExamine的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TaskExamineEditDto> CreateOrUpdateTaskExamineAsync(TaskExamineEditDto input)
        {

            if (input.Id.HasValue)
            {
                return await UpdateTaskExamineAsync(input);
            }
            else
            {
                return await CreateTaskExamineAsync(input);
            }
        }


        /// <summary>
        /// 新增TaskExamine
        /// </summary>
        protected virtual async Task<TaskExamineEditDto> CreateTaskExamineAsync(TaskExamineEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<TaskExamine>(input);
            var id = await _taskexamineRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<TaskExamineEditDto>();
        }

        /// <summary>
        /// 编辑TaskExamine
        /// </summary>
        protected virtual async Task<TaskExamineEditDto> UpdateTaskExamineAsync(TaskExamineEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _taskexamineRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            // ObjectMapper.Map(input, entity);
            var result = await _taskexamineRepository.UpdateAsync(entity);
            return result.MapTo<TaskExamineEditDto>();
        }



        /// <summary>
        /// 删除TaskExamine信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(TaskExamineAppPermissions.TaskExamine_Delete)]
        public async Task DeleteTaskExamine(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _taskexamineRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除TaskExamine的方法
        /// </summary>
        [AbpAuthorize(TaskExamineAppPermissions.TaskExamine_BatchDelete)]
        public async Task BatchDeleteTaskExaminesAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _taskexamineRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 删除任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task TaskExaminesDeleteByIdAsync(TaskExamineEditDto input)
        {
            var entity = await _taskexamineRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _taskexamineRepository.UpdateAsync(entity);
        }

    }
}


