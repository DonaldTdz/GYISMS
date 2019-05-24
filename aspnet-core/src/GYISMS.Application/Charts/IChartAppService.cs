using Abp.Application.Services;
using GYISMS.Charts.Dtos;
using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GYISMS.Charts
{
    public interface IChartAppService : IApplicationService
    {
        Task<List<ScheduleSummaryDto>> GetScheduleSummaryAsync(string userId, AreaCodeEnum areaCode);

        Task<List<ScheduleSummaryDto>> GetUserScheduleSummaryAsync(string userId);

        Task<DistrictChartDto> GetDistrictChartDataAsync(string userId, DateTime? startDate, DateTime? endDate, int tabIndex, AreaCodeEnum areaCode);

        Task<ChartDataDto> GetDeptChartDataAsync(long deptIdOrAreaCode,string userId, DateTime? startDate, DateTime? endDate, int tabIndex, string type);

        Task<ChartByTaskDto> GetChartByGroupAsync(DateTime? startTime, DateTime? endTime, int tabIndex, AreaCodeEnum areaCode);

        Task<ChartByTaskDto> GetChartByMothAsync(int searchMoth, AreaCodeEnum areaCode);

        Task<List<SheduleDetailDto>> GetSheduleDetail(int PageIndex, int TabIndex, string DateString, AreaCodeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus, string EmployeeId);

        Task<List<DistrictStatisDto>> GetSheduleDetailGroupArea(string DateString, int TabIndex, AreaCodeEnum? AreaCode, DateTime? StartTime, DateTime? EndTime, int? TaskId, int? Status, int? TStatus);
        Task<ChartByTaskDto> GetTaskCommChartByGroupAsync(long deptIdOrAreaCode, DateTime? startTime, DateTime? endTime, int tabIndex, string type);
        Task<DistrictChartByStatusDto> GetStatusChartDataAsync(AreaCodeEnum areaCode, int Status);
        Task<ChartByStatusDto> GetStatusTaskCommChartAsync(long deptIdOrAreaCode, string type, int Status);
    }
}
