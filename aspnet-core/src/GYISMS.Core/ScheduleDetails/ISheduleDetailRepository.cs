using Abp.Domain.Repositories;
using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GYISMS.ScheduleDetails
{
    public interface ISheduleDetailRepository :IRepository<ScheduleDetail, Guid>
    {
        Task<List<SheduleStatisticalDto>> GetSheduleStatisticalDtosByMothAsync(DateTime startTime, DateTime endTime);
        Task<List<SheduleSumDto>> GetSheduleSum(AreaTypeEnum? Area, DateTime? StartTime, DateTime? EndTime, string TaskName);
    }
}
