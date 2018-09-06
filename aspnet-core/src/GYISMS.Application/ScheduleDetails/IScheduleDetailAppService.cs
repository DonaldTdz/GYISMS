

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleDetails;

namespace GYISMS.ScheduleDetails
{
    /// <summary>
    /// ScheduleDetail应用层服务的接口方法
    ///</summary>
    public interface IScheduleDetailAppService : IApplicationService
    {
        /// <summary>
    /// 获取ScheduleDetail的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ScheduleDetailListDto>> GetPagedScheduleDetails(GetScheduleDetailsInput input);

		/// <summary>
		/// 通过指定id获取ScheduleDetailListDto信息
		/// </summary>
		Task<ScheduleDetailListDto> GetScheduleDetailByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出ScheduleDetail为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetScheduleDetailsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetScheduleDetailForEditOutput> GetScheduleDetailForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetScheduleDetailForEditOutput


        /// <summary>
        /// 添加或者修改ScheduleDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateScheduleDetail(CreateOrUpdateScheduleDetailInput input);


        /// <summary>
        /// 删除ScheduleDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteScheduleDetail(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ScheduleDetail
        /// </summary>
        Task BatchDeleteScheduleDetailsAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
