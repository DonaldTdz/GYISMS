

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.Growers.Dtos;
using GYISMS.Growers;

namespace GYISMS.Growers
{
    /// <summary>
    /// Grower应用层服务的接口方法
    ///</summary>
    public interface IGrowerAppService : IApplicationService
    {
        /// <summary>
    /// 获取Grower的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GrowerListDto>> GetPagedGrowersAsync(GetGrowersInput input);


        /// <summary>
        /// 导出Grower为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetGrowersToExcel();


        /// <summary>
        /// 添加或者修改Grower的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateGrower(CreateOrUpdateGrowerInput input);


        /// <summary>
        /// 删除Grower信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteGrower(EntityDto<int> input);


        /// <summary>
        /// 批量删除Grower
        /// </summary>
        Task BatchDeleteGrowersAsync(List<int> input);


        Task<GrowerEditDto> CreateOrUpdateGrowerAsycn(GrowerEditDto input);
        Task GrowerDeleteByIdAsync(GrowerEditDto input);
        Task<GrowerListDto> GetGrowerByIdAsync(int id);
    }
}
