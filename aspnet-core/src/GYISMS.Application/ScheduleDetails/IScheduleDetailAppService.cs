

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleDetails;
using GYISMS.Growers.Dtos;
using GYISMS.Dtos;

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
        /// 删除ScheduleDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteScheduleDetail(Guid Id);


        /// <summary>
        /// 批量删除ScheduleDetail
        /// </summary>
        Task BatchDeleteScheduleDetailsAsync(List<Guid> input);

        Task<List<ScheduleDetailEditDto>> CreateOrUpdateScheduleTaskAsync(List<ScheduleDetailEditDto> input);

        HomeInfo GetHomeInfo();

        Task<List<SheduleStatisticalDto>> GetSchedulByAreaTime(ScheduleDetaStatisticalInput input);

        Task<List<SheduleStatisticalDto>> GetSchedulByMothTime(int input);
        Task<APIResultDto> CreateAllScheduleTaskAsync(GetGrowersInput input);

        Task<SheduleSumStatisDto> GetSumShedule(SheduleSumInput input);

        Task<PagedResultDto<SheduleDetailTaskListDto>> GetPagedScheduleDetailsByOtherTable(GetScheduleDetailsInput input);

        Task AutoUpdateOverdueStatusAsync();

        Task SendTaskOverdueMsgAsync();

        Task<APIResultDto> ExportSheduleSumExcel(SheduleSumInput input);

        Task<APIResultDto> ExportSheduleDetailExcel(GetScheduleDetailsInput input);
        Task<PagedResultDto<ScheduleDetailListDto>> GetPagedScheduleDetailRecordAsync(GetScheduleDetailsInput input);
    }
}
