

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.VisitTasks.Dtos;
using GYISMS.VisitTasks;
using GYISMS.TaskExamines.Dtos;

namespace GYISMS.VisitTasks
{
    /// <summary>
    /// VisitTask应用层服务的接口方法
    ///</summary>
    public interface IVisitTaskAppService : IApplicationService
    {
        /// <summary>
    /// 获取VisitTask的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VisitTaskListDto>> GetPagedVisitTasksAsync(GetVisitTasksInput input);

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVisitTaskForEditOutput> GetVisitTaskForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 删除VisitTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteVisitTask(EntityDto<int> input);


        /// <summary>
        /// 批量删除VisitTask
        /// </summary>
        Task BatchDeleteVisitTasksAsync(List<int> input);
        Task VisitTaskDeleteByIdAsync(VisitTaskEditDto input);
        Task<VisitTaskListDto> GetVisitTaskByIdAsync(int id);
        Task<VisitTaskListDto> CreateOrUpdateVisitTaskAsycn(VisitTaskEditDto input);
    }
}
