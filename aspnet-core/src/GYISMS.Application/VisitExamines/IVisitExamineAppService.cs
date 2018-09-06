

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.VisitExamines.Dtos;
using GYISMS.VisitExamines;

namespace GYISMS.VisitExamines
{
    /// <summary>
    /// VisitExamine应用层服务的接口方法
    ///</summary>
    public interface IVisitExamineAppService : IApplicationService
    {
        /// <summary>
    /// 获取VisitExamine的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VisitExamineListDto>> GetPagedVisitExamines(GetVisitExaminesInput input);

		/// <summary>
		/// 通过指定id获取VisitExamineListDto信息
		/// </summary>
		Task<VisitExamineListDto> GetVisitExamineByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出VisitExamine为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetVisitExaminesToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVisitExamineForEditOutput> GetVisitExamineForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetVisitExamineForEditOutput


        /// <summary>
        /// 添加或者修改VisitExamine的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateVisitExamine(CreateOrUpdateVisitExamineInput input);


        /// <summary>
        /// 删除VisitExamine信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteVisitExamine(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除VisitExamine
        /// </summary>
        Task BatchDeleteVisitExaminesAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
