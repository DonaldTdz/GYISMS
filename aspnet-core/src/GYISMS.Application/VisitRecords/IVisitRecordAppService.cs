

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.VisitRecords.Dtos;
using GYISMS.VisitRecords;
using GYISMS.Dtos;

namespace GYISMS.VisitRecords
{
    /// <summary>
    /// VisitRecord应用层服务的接口方法
    ///</summary>
    public interface IVisitRecordAppService : IApplicationService
    {
        /// <summary>
    /// 获取VisitRecord的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VisitRecordListDto>> GetPagedVisitRecords(GetVisitRecordsInput input);

		/// <summary>
		/// 通过指定id获取VisitRecordListDto信息
		/// </summary>
		Task<VisitRecordListDto> GetVisitRecordByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出VisitRecord为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetVisitRecordsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVisitRecordForEditOutput> GetVisitRecordForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetVisitRecordForEditOutput


        /// <summary>
        /// 添加或者修改VisitRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateVisitRecord(CreateOrUpdateVisitRecordInput input);


        /// <summary>
        /// 删除VisitRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteVisitRecord(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除VisitRecord
        /// </summary>
        Task BatchDeleteVisitRecordsAsync(List<Guid> input);

        Task<DingDingVisitRecordInputDto> GetCreateDingDingVisitRecordAsync(Guid scheduleDetailId);

        Task<APIResultDto> SaveDingDingVisitRecordAsync(DingDingVisitRecordInputDto input);
        Task<PagedResultDto<VisitRecordListDto>> GetVisitRecordsByGrowerId(GetVisitRecordsInput input);

        Task GenerateWatermarkImgTests();
    }
}
