
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


using GYISMS.GrowerAreaRecords.Dtos;
using GYISMS.GrowerAreaRecords;
using GYISMS.GYEnums;

namespace GYISMS.GrowerAreaRecords
{
    /// <summary>
    /// GrowerAreaRecord应用层服务的接口方法
    ///</summary>
    public interface IGrowerAreaRecordAppService : IApplicationService
    {
        /// <summary>
		/// 获取GrowerAreaRecord的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GrowerAreaRecordListDto>> GetPaged(GetGrowerAreaRecordsInput input);


		/// <summary>
		/// 通过指定id获取GrowerAreaRecordListDto信息
		/// </summary>
		Task<GrowerAreaRecordListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetGrowerAreaRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改GrowerAreaRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateGrowerAreaRecordInput input);


        /// <summary>
        /// 删除GrowerAreaRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除GrowerAreaRecord
        /// </summary>
        Task BatchDelete(List<Guid> input);

        Task<CityAreaChartDto> GetCityDDChartDataAsync();

        Task<DistrictAreaChartDto> GetDistrictDDChartDataAsync();
        Task<CommDetail> GetAreaOrganization(EntityDto<int> input);
        /// <summary>
        /// 新增记录
        /// </summary>
        Task SaveGrowerAreaRecordAsync(DingDingAreaRecordInput input);
        /// <summary>
        /// 落实面积提交
        /// </summary>
        Task SubmitGrowerAreaAsync(EntityDto<Guid> input);

        Task PostDeleteAsync(EntityDto<Guid> input);
    }
}
