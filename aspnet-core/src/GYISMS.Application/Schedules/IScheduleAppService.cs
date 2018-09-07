

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.Schedules.Dtos;
using GYISMS.Schedules;

namespace GYISMS.Schedules
{
    /// <summary>
    /// Schedule应用层服务的接口方法
    ///</summary>
    public interface IScheduleAppService : IApplicationService
    {
        /// <summary>
    /// 获取Schedule的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ScheduleListDto>> GetPagedSchedules(GetSchedulesInput input);

		/// <summary>
		/// 通过指定id获取ScheduleListDto信息
		/// </summary>
		Task<ScheduleListDto> GetScheduleByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出Schedule为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetSchedulesToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetScheduleForEditOutput> GetScheduleForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetScheduleForEditOutput


        /// <summary>
        /// 添加或者修改Schedule的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateSchedule(CreateOrUpdateScheduleInput input);


        /// <summary>
        /// 删除Schedule信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSchedule(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Schedule
        /// </summary>
        Task BatchDeleteSchedulesAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
