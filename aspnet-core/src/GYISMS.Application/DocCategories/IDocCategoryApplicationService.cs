
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


using GYISMS.DocCategories.Dtos;
using GYISMS.DocCategories;
using GYISMS.Dtos;

namespace GYISMS.DocCategories
{
    /// <summary>
    /// DocCategory应用层服务的接口方法
    ///</summary>
    public interface IDocCategoryAppService : IApplicationService
    {
        /// <summary>
		/// 获取DocCategory的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocCategoryListDto>> GetPaged(GetDocCategorysInput input);


		/// <summary>
		/// 通过指定id获取DocCategoryListDto信息
		/// </summary>
		Task<DocCategoryListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDocCategoryForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改DocCategory的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDocCategoryInput input);


        /// <summary>
        /// 删除DocCategory信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除DocCategory
        /// </summary>
        Task BatchDelete(List<int> input);


        /// <summary>
        /// 导出DocCategory为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

        Task<List<CategoryTreeNode>> GetTreeAsync(long? deptId);
        Task<List<GridListDto>> GetCategoryListAsync(string host,string userId);
        Task<List<TabListDto>> GetTabChildListByIdAsync(int id);
        Task<string> GetParentName(int id);
        Task<APIResultDto> CategoryRemoveById(EntityDto<int> id);
        Task<APIResultDto> CopyCategoryByDeptIdAsync(CopyInput input);
        Task<List<CategoryTreeNode>> GetCopyTreeWithRootAsync(long? deptId);
    }
}
