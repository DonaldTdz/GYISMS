
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
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using GYISMS.GrowerAreaRecords;
using GYISMS.GrowerAreaRecords.Dtos;




namespace GYISMS.GrowerAreaRecords
{
    /// <summary>
    /// GrowerAreaRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class GrowerAreaRecordAppService : GYISMSAppServiceBase, IGrowerAreaRecordAppService
    {
        private readonly IRepository<GrowerAreaRecord, Guid> _entityRepository;

        

        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAreaRecordAppService(
        IRepository<GrowerAreaRecord, Guid> entityRepository
        
        )
        {
            _entityRepository = entityRepository; 
            
        }


        /// <summary>
        /// 获取GrowerAreaRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<GrowerAreaRecordListDto>> GetPaged(GetGrowerAreaRecordsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<GrowerAreaRecordListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<GrowerAreaRecordListDto>>();

			return new PagedResultDto<GrowerAreaRecordListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取GrowerAreaRecordListDto信息
		/// </summary>
		 
		public async Task<GrowerAreaRecordListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<GrowerAreaRecordListDto>();
		}

		/// <summary>
		/// 获取编辑 GrowerAreaRecord
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetGrowerAreaRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetGrowerAreaRecordForEditOutput();
GrowerAreaRecordEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<GrowerAreaRecordEditDto>();

				//growerAreaRecordEditDto = ObjectMapper.Map<List<growerAreaRecordEditDto>>(entity);
			}
			else
			{
				editDto = new GrowerAreaRecordEditDto();
			}

			output.GrowerAreaRecord = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改GrowerAreaRecord的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateGrowerAreaRecordInput input)
		{

			if (input.GrowerAreaRecord.Id.HasValue)
			{
				await Update(input.GrowerAreaRecord);
			}
			else
			{
				await Create(input.GrowerAreaRecord);
			}
		}


		/// <summary>
		/// 新增GrowerAreaRecord
		/// </summary>
		
		protected virtual async Task<GrowerAreaRecordEditDto> Create(GrowerAreaRecordEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <GrowerAreaRecord>(input);
            var entity=input.MapTo<GrowerAreaRecord>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<GrowerAreaRecordEditDto>();
		}

		/// <summary>
		/// 编辑GrowerAreaRecord
		/// </summary>
		
		protected virtual async Task Update(GrowerAreaRecordEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除GrowerAreaRecord信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除GrowerAreaRecord的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出GrowerAreaRecord为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetToExcel()
		//{
		//	var users = await UserManager.Users.ToListAsync();
		//	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
		//	await FillRoleNames(userListDtos);
		//	return _userListExcelExporter.ExportToFile(userListDtos);
		//}

    }
}


