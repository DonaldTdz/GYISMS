

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.SystemDatas.Dtos;
using GYISMS.SystemDatas;

namespace GYISMS.SystemDatas
{
    /// <summary>
    /// SystemData应用层服务的接口方法
    ///</summary>
    public interface ISystemDataAppService : IApplicationService
    {
        /// <summary>
    /// 获取SystemData的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SystemDataListDto>> GetPagedSystemDatas(GetSystemDatasInput input);

		/// <summary>
		/// 通过指定id获取SystemDataListDto信息
		/// </summary>
		Task<SystemDataListDto> GetSystemDataByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 导出SystemData为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetSystemDatasToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetSystemDataForEditOutput> GetSystemDataForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetSystemDataForEditOutput


        /// <summary>
        /// 添加或者修改SystemData的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateSystemData(CreateOrUpdateSystemDataInput input);


        /// <summary>
        /// 删除SystemData信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteSystemData(EntityDto<int> input);


        /// <summary>
        /// 批量删除SystemData
        /// </summary>
        Task BatchDeleteSystemDatasAsync(List<int> input);


		//// custom codes
		 
        //// custom codes end
    }
}
