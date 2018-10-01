using Abp.Application.Services;
using GYISMS.Charts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GYISMS.Charts
{
    public interface IChartAppService : IApplicationService
    {
        Task<List<ScheduleSummaryDto>> GetScheduleSummaryAsync(string userId);

        Task<DistrictChartDto> GetDistrictChartDataAsync(string userId, DateTime? startDate, DateTime? endDate);
    }
}
