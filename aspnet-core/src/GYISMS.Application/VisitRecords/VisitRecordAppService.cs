
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

using GYISMS.VisitRecords.Authorization;
using GYISMS.VisitRecords.Dtos;
using GYISMS.VisitRecords;
using GYISMS.Authorization;

namespace GYISMS.VisitRecords
{
    /// <summary>
    /// VisitRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class VisitRecordAppService : GYISMSAppServiceBase, IVisitRecordAppService
    {
        private readonly IRepository<VisitRecord, Guid>
        _visitrecordRepository;


        private readonly IVisitRecordManager _visitrecordManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public VisitRecordAppService(
        IRepository<VisitRecord, Guid>
    visitrecordRepository
            , IVisitRecordManager visitrecordManager
            )
        {
            _visitrecordRepository = visitrecordRepository;
            _visitrecordManager = visitrecordManager;
        }


        /// <summary>
        /// 获取VisitRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VisitRecordListDto>> GetPagedVisitRecords(GetVisitRecordsInput input)
        {

            var query = _visitrecordRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var visitrecordCount = await query.CountAsync();

            var visitrecords = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var visitrecordListDtos = ObjectMapper.Map<List <VisitRecordListDto>>(visitrecords);
            var visitrecordListDtos = visitrecords.MapTo<List<VisitRecordListDto>>();

            return new PagedResultDto<VisitRecordListDto>(
visitrecordCount,
visitrecordListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取VisitRecordListDto信息
        /// </summary>
        public async Task<VisitRecordListDto> GetVisitRecordByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _visitrecordRepository.GetAsync(input.Id);

            return entity.MapTo<VisitRecordListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetVisitRecordForEditOutput> GetVisitRecordForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetVisitRecordForEditOutput();
            VisitRecordEditDto visitrecordEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _visitrecordRepository.GetAsync(input.Id.Value);

                visitrecordEditDto = entity.MapTo<VisitRecordEditDto>();

                //visitrecordEditDto = ObjectMapper.Map<List <visitrecordEditDto>>(entity);
            }
            else
            {
                visitrecordEditDto = new VisitRecordEditDto();
            }

            output.VisitRecord = visitrecordEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改VisitRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateVisitRecord(CreateOrUpdateVisitRecordInput input)
        {

            if (input.VisitRecord.Id.HasValue)
            {
                await UpdateVisitRecordAsync(input.VisitRecord);
            }
            else
            {
                await CreateVisitRecordAsync(input.VisitRecord);
            }
        }


        /// <summary>
        /// 新增VisitRecord
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Create)]
        protected virtual async Task<VisitRecordEditDto> CreateVisitRecordAsync(VisitRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<VisitRecord>(input);

            entity = await _visitrecordRepository.InsertAsync(entity);
            return entity.MapTo<VisitRecordEditDto>();
        }

        /// <summary>
        /// 编辑VisitRecord
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Edit)]
        protected virtual async Task UpdateVisitRecordAsync(VisitRecordEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _visitrecordRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _visitrecordRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除VisitRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Delete)]
        public async Task DeleteVisitRecord(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _visitrecordRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除VisitRecord的方法
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_BatchDelete)]
        public async Task BatchDeleteVisitRecordsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _visitrecordRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出VisitRecord为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetVisitRecordsToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}



        //// custom codes

        //// custom codes end

    }
}


