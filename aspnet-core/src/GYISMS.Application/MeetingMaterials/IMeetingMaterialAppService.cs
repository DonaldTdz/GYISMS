

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.MeetingMaterials.Dtos;
using GYISMS.MeetingMaterials;

namespace GYISMS.MeetingMaterials
{
    /// <summary>
    /// MeetingMaterial应用层服务的接口方法
    ///</summary>
    public interface IMeetingMaterialAppService : IApplicationService
    {
        /// <summary>
    /// 获取MeetingMaterial的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingMaterialListDto>> GetPagedMeetingMaterials(GetMeetingMaterialsInput input);

		/// <summary>
		/// 通过指定id获取MeetingMaterialListDto信息
		/// </summary>
		Task<MeetingMaterialListDto> GetMeetingMaterialByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出MeetingMaterial为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetMeetingMaterialsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMeetingMaterialForEditOutput> GetMeetingMaterialForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetMeetingMaterialForEditOutput


        /// <summary>
        /// 添加或者修改MeetingMaterial的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateMeetingMaterial(CreateOrUpdateMeetingMaterialInput input);


        /// <summary>
        /// 删除MeetingMaterial信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMeetingMaterial(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除MeetingMaterial
        /// </summary>
        Task BatchDeleteMeetingMaterialsAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
