
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

using GYISMS.SystemDatas.Authorization;
using GYISMS.SystemDatas.Dtos;
using GYISMS.SystemDatas;
using GYISMS.Authorization;

namespace GYISMS.SystemDatas
{
    /// <summary>
    /// SystemData应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class SystemDataAppService : GYISMSAppServiceBase, ISystemDataAppService
    {
    private readonly IRepository<SystemData, int>
    _systemdataRepository;
    
       
       private readonly ISystemDataManager _systemdataManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public SystemDataAppService(
    IRepository<SystemData, int>
systemdataRepository
        ,ISystemDataManager systemdataManager
        )
        {
        _systemdataRepository = systemdataRepository;
  _systemdataManager=systemdataManager;
        }


        /// <summary>
            /// 获取SystemData的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<SystemDataListDto>> GetPagedSystemDatas(GetSystemDatasInput input)
		{

		    var query = _systemdataRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var systemdataCount = await query.CountAsync();

			var systemdatas = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var systemdataListDtos = ObjectMapper.Map<List <SystemDataListDto>>(systemdatas);
				var systemdataListDtos =systemdatas.MapTo<List<SystemDataListDto>>();

				return new PagedResultDto<SystemDataListDto>(
systemdataCount,
systemdataListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取SystemDataListDto信息
		/// </summary>
		public async Task<SystemDataListDto> GetSystemDataByIdAsync(EntityDto<int> input)
		{
			var entity = await _systemdataRepository.GetAsync(input.Id);

		    return entity.MapTo<SystemDataListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetSystemDataForEditOutput> GetSystemDataForEdit(NullableIdDto<int> input)
		{
			var output = new GetSystemDataForEditOutput();
SystemDataEditDto systemdataEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _systemdataRepository.GetAsync(input.Id.Value);

systemdataEditDto = entity.MapTo<SystemDataEditDto>();

				//systemdataEditDto = ObjectMapper.Map<List <systemdataEditDto>>(entity);
			}
			else
			{
systemdataEditDto = new SystemDataEditDto();
			}

			output.SystemData = systemdataEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改SystemData的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateSystemData(CreateOrUpdateSystemDataInput input)
		{

			if (input.SystemData.Id.HasValue)
			{
				await UpdateSystemDataAsync(input.SystemData);
			}
			else
			{
				await CreateSystemDataAsync(input.SystemData);
			}
		}


		/// <summary>
		/// 新增SystemData
		/// </summary>
		[AbpAuthorize(SystemDataAppPermissions.SystemData_Create)]
		protected virtual async Task<SystemDataEditDto> CreateSystemDataAsync(SystemDataEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <SystemData>(input);

			entity = await _systemdataRepository.InsertAsync(entity);
			return entity.MapTo<SystemDataEditDto>();
		}

		/// <summary>
		/// 编辑SystemData
		/// </summary>
		[AbpAuthorize(SystemDataAppPermissions.SystemData_Edit)]
		protected virtual async Task UpdateSystemDataAsync(SystemDataEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _systemdataRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _systemdataRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除SystemData信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(SystemDataAppPermissions.SystemData_Delete)]
		public async Task DeleteSystemData(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _systemdataRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除SystemData的方法
		/// </summary>
		          [AbpAuthorize(SystemDataAppPermissions.SystemData_BatchDelete)]
		public async Task BatchDeleteSystemDatasAsync(List<int> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _systemdataRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出SystemData为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetSystemDatasToExcel()
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


