

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.TaskExamines.Dtos;
using GYISMS.TaskExamines;

namespace GYISMS.TaskExamines
{
    /// <summary>
    /// TaskExamine应用层服务的接口方法
    ///</summary>
    public interface ITaskExamineAppService : IApplicationService
    {
        /// <summary>
    /// 获取TaskExamine的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TaskExamineListDto>> GetPagedTaskExaminesAsync(GetTaskExaminesInput input);

        /// <summary>
        /// 通过指定id获取TaskExamineListDto信息
        /// </summary>
        Task<TaskExamineListDto> GetTaskExamineByIdAsync(int id);


        /// <summary>
        /// 导出TaskExamine为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetTaskExaminesToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTaskExamineForEditOutput> GetTaskExamineForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetTaskExamineForEditOutput


        /// <summary>
        /// 添加或者修改TaskExamine的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TaskExamineEditDto> CreateOrUpdateTaskExamineAsync(TaskExamineEditDto input);


        /// <summary>
        /// 删除TaskExamine信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteTaskExamine(EntityDto<int> input);


        /// <summary>
        /// 批量删除TaskExamine
        /// </summary>
        Task BatchDeleteTaskExaminesAsync(List<int> input);

        Task TaskExaminesDeleteByIdAsync(TaskExamineEditDto input);
        //// custom codes

        //// custom codes end
    }
}
