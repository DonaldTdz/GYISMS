

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.ScheduleTasks.Dtos;
using GYISMS.ScheduleTasks;

namespace GYISMS.ScheduleTasks
{
    /// <summary>
    /// ScheduleTask应用层服务的接口方法
    ///</summary>
    public interface IScheduleTaskAppService : IApplicationService
    {
        /// <summary>
    /// 获取ScheduleTask的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ScheduleTaskListDto>> GetPagedScheduleTasks(GetScheduleTasksInput input);

		/// <summary>
		/// 通过指定id获取ScheduleTaskListDto信息
		/// </summary>
		Task<ScheduleTaskListDto> GetScheduleTaskByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出ScheduleTask为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetScheduleTasksToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetScheduleTaskForEditOutput> GetScheduleTaskForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetScheduleTaskForEditOutput


        /// <summary>
        /// 添加或者修改ScheduleTask的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateScheduleTask(CreateOrUpdateScheduleTaskInput input);


        /// <summary>
        /// 删除ScheduleTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteScheduleTask(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ScheduleTask
        /// </summary>
        Task BatchDeleteScheduleTasksAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
