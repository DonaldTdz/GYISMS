
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

using GYISMS.VisitExamines.Authorization;
using GYISMS.VisitExamines.Dtos;
using GYISMS.VisitExamines;
using GYISMS.Authorization;

namespace GYISMS.VisitExamines
{
    /// <summary>
    /// VisitExamine应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class VisitExamineAppService : GYISMSAppServiceBase, IVisitExamineAppService
    {
    private readonly IRepository<VisitExamine, Guid>
    _visitexamineRepository;
    
       
       private readonly IVisitExamineManager _visitexamineManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public VisitExamineAppService(
    IRepository<VisitExamine, Guid>
visitexamineRepository
        ,IVisitExamineManager visitexamineManager
        )
        {
        _visitexamineRepository = visitexamineRepository;
  _visitexamineManager=visitexamineManager;
        }


        /// <summary>
            /// 获取VisitExamine的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<VisitExamineListDto>> GetPagedVisitExamines(GetVisitExaminesInput input)
		{

		    var query = _visitexamineRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var visitexamineCount = await query.CountAsync();

			var visitexamines = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var visitexamineListDtos = ObjectMapper.Map<List <VisitExamineListDto>>(visitexamines);
				var visitexamineListDtos =visitexamines.MapTo<List<VisitExamineListDto>>();

				return new PagedResultDto<VisitExamineListDto>(
visitexamineCount,
visitexamineListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取VisitExamineListDto信息
		/// </summary>
		public async Task<VisitExamineListDto> GetVisitExamineByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _visitexamineRepository.GetAsync(input.Id);

		    return entity.MapTo<VisitExamineListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetVisitExamineForEditOutput> GetVisitExamineForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetVisitExamineForEditOutput();
VisitExamineEditDto visitexamineEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _visitexamineRepository.GetAsync(input.Id.Value);

visitexamineEditDto = entity.MapTo<VisitExamineEditDto>();

				//visitexamineEditDto = ObjectMapper.Map<List <visitexamineEditDto>>(entity);
			}
			else
			{
visitexamineEditDto = new VisitExamineEditDto();
			}

			output.VisitExamine = visitexamineEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改VisitExamine的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateVisitExamine(CreateOrUpdateVisitExamineInput input)
		{

			if (input.VisitExamine.Id.HasValue)
			{
				await UpdateVisitExamineAsync(input.VisitExamine);
			}
			else
			{
				await CreateVisitExamineAsync(input.VisitExamine);
			}
		}


		/// <summary>
		/// 新增VisitExamine
		/// </summary>
		[AbpAuthorize(VisitExamineAppPermissions.VisitExamine_Create)]
		protected virtual async Task<VisitExamineEditDto> CreateVisitExamineAsync(VisitExamineEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <VisitExamine>(input);

			entity = await _visitexamineRepository.InsertAsync(entity);
			return entity.MapTo<VisitExamineEditDto>();
		}

		/// <summary>
		/// 编辑VisitExamine
		/// </summary>
		[AbpAuthorize(VisitExamineAppPermissions.VisitExamine_Edit)]
		protected virtual async Task UpdateVisitExamineAsync(VisitExamineEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _visitexamineRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _visitexamineRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除VisitExamine信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(VisitExamineAppPermissions.VisitExamine_Delete)]
		public async Task DeleteVisitExamine(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _visitexamineRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除VisitExamine的方法
		/// </summary>
		          [AbpAuthorize(VisitExamineAppPermissions.VisitExamine_BatchDelete)]
		public async Task BatchDeleteVisitExaminesAsync(List<Guid> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _visitexamineRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出VisitExamine为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetVisitExaminesToExcel()
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


