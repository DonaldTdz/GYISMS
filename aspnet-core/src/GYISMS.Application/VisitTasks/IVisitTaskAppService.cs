

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.VisitTasks.Dtos;
using GYISMS.VisitTasks;

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
        Task<PagedResultDto<VisitTaskListDto>> GetPagedVisitTasks(GetVisitTasksInput input);

		/// <summary>
		/// 通过指定id获取VisitTaskListDto信息
		/// </summary>
		Task<VisitTaskListDto> GetVisitTaskByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 导出VisitTask为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetVisitTasksToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVisitTaskForEditOutput> GetVisitTaskForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetVisitTaskForEditOutput


        /// <summary>
        /// 添加或者修改VisitTask的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateVisitTask(CreateOrUpdateVisitTaskInput input);


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


		//// custom codes
		 
        //// custom codes end
    }
}
