
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

namespace GYISMS.Schedules
{
    /// <summary>
    /// Schedule应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]

    public class ScheduleAppService : GYISMSAppServiceBase, IScheduleAppService
    {
        private readonly IRepository<Schedule, Guid>
        _scheduleRepository;


        private readonly IScheduleManager _scheduleManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleAppService(
        IRepository<Schedule, Guid>
    scheduleRepository
            , IScheduleManager scheduleManager
            )
        {
            _scheduleRepository = scheduleRepository;
            _scheduleManager = scheduleManager;
        }


        /// <summary>
        /// 获取Schedule的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ScheduleListDto>> GetPagedSchedulesAsync(GetSchedulesInput input)
        {

            var query = _scheduleRepository.GetAll().Where(v => v.IsDeleted == false)
                     .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                     .WhereIf(input.ScheduleType.HasValue, r => r.Type == input.ScheduleType);;
            // TODO:根据传入的参数添加过滤条件

            var scheduleCount = await query.CountAsync();

            var schedules = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var scheduleListDtos = ObjectMapper.Map<List <ScheduleListDto>>(schedules);
            var scheduleListDtos = schedules.MapTo<List<ScheduleListDto>>();

            return new PagedResultDto<ScheduleListDto>(
                    scheduleCount,
                    scheduleListDtos
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
    }
}