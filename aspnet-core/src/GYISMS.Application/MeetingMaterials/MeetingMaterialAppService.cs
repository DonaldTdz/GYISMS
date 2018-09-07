
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

using GYISMS.MeetingMaterials.Authorization;
using GYISMS.MeetingMaterials.Dtos;
using GYISMS.MeetingMaterials;
using GYISMS.Authorization;

namespace GYISMS.MeetingMaterials
{
    /// <summary>
    /// MeetingMaterial应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class MeetingMaterialAppService : GYISMSAppServiceBase, IMeetingMaterialAppService
    {
    private readonly IRepository<MeetingMaterial, Guid>
    _meetingmaterialRepository;
    
       
       private readonly IMeetingMaterialManager _meetingmaterialManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public MeetingMaterialAppService(
    IRepository<MeetingMaterial, Guid>
meetingmaterialRepository
        ,IMeetingMaterialManager meetingmaterialManager
        )
        {
        _meetingmaterialRepository = meetingmaterialRepository;
  _meetingmaterialManager=meetingmaterialManager;
        }


        /// <summary>
            /// 获取MeetingMaterial的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<MeetingMaterialListDto>> GetPagedMeetingMaterials(GetMeetingMaterialsInput input)
		{

		    var query = _meetingmaterialRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var meetingmaterialCount = await query.CountAsync();

			var meetingmaterials = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var meetingmaterialListDtos = ObjectMapper.Map<List <MeetingMaterialListDto>>(meetingmaterials);
				var meetingmaterialListDtos =meetingmaterials.MapTo<List<MeetingMaterialListDto>>();

				return new PagedResultDto<MeetingMaterialListDto>(
meetingmaterialCount,
meetingmaterialListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取MeetingMaterialListDto信息
		/// </summary>
		public async Task<MeetingMaterialListDto> GetMeetingMaterialByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _meetingmaterialRepository.GetAsync(input.Id);

		    return entity.MapTo<MeetingMaterialListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetMeetingMaterialForEditOutput> GetMeetingMaterialForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetMeetingMaterialForEditOutput();
MeetingMaterialEditDto meetingmaterialEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _meetingmaterialRepository.GetAsync(input.Id.Value);

meetingmaterialEditDto = entity.MapTo<MeetingMaterialEditDto>();

				//meetingmaterialEditDto = ObjectMapper.Map<List <meetingmaterialEditDto>>(entity);
			}
			else
			{
meetingmaterialEditDto = new MeetingMaterialEditDto();
			}

			output.MeetingMaterial = meetingmaterialEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改MeetingMaterial的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateMeetingMaterial(CreateOrUpdateMeetingMaterialInput input)
		{

			if (input.MeetingMaterial.Id.HasValue)
			{
				await UpdateMeetingMaterialAsync(input.MeetingMaterial);
			}
			else
			{
				await CreateMeetingMaterialAsync(input.MeetingMaterial);
			}
		}


		/// <summary>
		/// 新增MeetingMaterial
		/// </summary>
		[AbpAuthorize(MeetingMaterialAppPermissions.MeetingMaterial_Create)]
		protected virtual async Task<MeetingMaterialEditDto> CreateMeetingMaterialAsync(MeetingMaterialEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <MeetingMaterial>(input);

			entity = await _meetingmaterialRepository.InsertAsync(entity);
			return entity.MapTo<MeetingMaterialEditDto>();
		}

		/// <summary>
		/// 编辑MeetingMaterial
		/// </summary>
		[AbpAuthorize(MeetingMaterialAppPermissions.MeetingMaterial_Edit)]
		protected virtual async Task UpdateMeetingMaterialAsync(MeetingMaterialEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _meetingmaterialRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _meetingmaterialRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除MeetingMaterial信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(MeetingMaterialAppPermissions.MeetingMaterial_Delete)]
		public async Task DeleteMeetingMaterial(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _meetingmaterialRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除MeetingMaterial的方法
		/// </summary>
		          [AbpAuthorize(MeetingMaterialAppPermissions.MeetingMaterial_BatchDelete)]
		public async Task BatchDeleteMeetingMaterialsAsync(List<Guid> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _meetingmaterialRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出MeetingMaterial为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetMeetingMaterialsToExcel()
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


