

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.ScheduleTasks.Dtos;
using GYISMS.ScheduleTasks;
using GYISMS.GYEnums;

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
        Task<PagedResultDto<ScheduleTaskListDto>> GetPagedScheduleTasksAsync(GetScheduleTasksInput input);


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

        //Task<ScheduleTaskEditDto> CreateOrUpdateScheduleTaskAsycn(ScheduleTaskEditDto input);
        Task<List<ScheduleTaskEditDto>> CreateOrUpdateScheduleTaskAsync(List<ScheduleTaskEditDto> input);


        Task<List<DingDingScheduleTaskDto>> GetDingDingScheduleTaskListAsync(string userId);

        Task<DingDingTaskDto> GetDingDingTaskInfoAsync(Guid scheduleTaskId, string uid, int Status);

        Task<DingDingVisitGrowerDetailDto> GetDingDingVisitGrowerDetailAsync(Guid scheduleDetailId);

        Task<List<ScheduleTaskListDto>> GetScheduleTasksNoPageAsync(Guid id);
        Task VisitTaskDeleteByIdAsync(ScheduleTaskEditDto input);

        Task<List<DingDingScheduleDetailDto>> GetDingDingScheduleTaskPagingAsync(string userId, ScheduleStatusEnum status, DateTime? startDate, DateTime? endDate, int pageIndex);

    }
}
