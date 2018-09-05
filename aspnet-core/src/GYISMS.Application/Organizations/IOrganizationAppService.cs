

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.Organizations.Dtos;
using GYISMS.Organizations;

namespace GYISMS.Organizations
{
    /// <summary>
    /// Organization应用层服务的接口方法
    ///</summary>
    public interface IOrganizationAppService : IApplicationService
    {
        /// <summary>
    /// 获取Organization的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationListDto>> GetPagedOrganizationsAsync(GetOrganizationsInput input);

		/// <summary>
		/// 通过指定id获取OrganizationListDto信息
		/// </summary>
		Task<OrganizationListDto> GetOrganizationByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 导出Organization为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetOrganizationsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetOrganizationForEditOutput> GetOrganizationForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetOrganizationForEditOutput


        /// <summary>
        /// 添加或者修改Organization的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateOrganization(CreateOrUpdateOrganizationInput input);


        /// <summary>
        /// 删除Organization信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteOrganization(EntityDto<int> input);


        /// <summary>
        /// 批量删除Organization
        /// </summary>
        Task BatchDeleteOrganizationsAsync(List<int> input);

        List<Organization> GetOrganization();

        //// custom codes

        //// custom codes end
    }
}
