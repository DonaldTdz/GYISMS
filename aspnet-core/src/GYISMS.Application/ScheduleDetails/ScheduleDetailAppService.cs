
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

using GYISMS.ScheduleDetails.Authorization;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleDetails;
using GYISMS.Authorization;

namespace GYISMS.ScheduleDetails
{
    /// <summary>
    /// ScheduleDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class ScheduleDetailAppService : GYISMSAppServiceBase, IScheduleDetailAppService
    {
    private readonly IRepository<ScheduleDetail, Guid>
    _scheduledetailRepository;
    
       
       private readonly IScheduleDetailManager _scheduledetailManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public ScheduleDetailAppService(
    IRepository<ScheduleDetail, Guid>
scheduledetailRepository
        ,IScheduleDetailManager scheduledetailManager
        )
        {
        _scheduledetailRepository = scheduledetailRepository;
  _scheduledetailManager=scheduledetailManager;
        }


        /// <summary>
            /// 获取ScheduleDetail的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<ScheduleDetailListDto>> GetPagedScheduleDetails(GetScheduleDetailsInput input)
		{

		    var query = _scheduledetailRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var scheduledetailCount = await query.CountAsync();

			var scheduledetails = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var scheduledetailListDtos = ObjectMapper.Map<List <ScheduleDetailListDto>>(scheduledetails);
				var scheduledetailListDtos =scheduledetails.MapTo<List<ScheduleDetailListDto>>();

				return new PagedResultDto<ScheduleDetailListDto>(
scheduledetailCount,
scheduledetailListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取ScheduleDetailListDto信息
		/// </summary>
		public async Task<ScheduleDetailListDto> GetScheduleDetailByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _scheduledetailRepository.GetAsync(input.Id);

		    return entity.MapTo<ScheduleDetailListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetScheduleDetailForEditOutput> GetScheduleDetailForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetScheduleDetailForEditOutput();
ScheduleDetailEditDto scheduledetailEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);

scheduledetailEditDto = entity.MapTo<ScheduleDetailEditDto>();

				//scheduledetailEditDto = ObjectMapper.Map<List <scheduledetailEditDto>>(entity);
			}
			else
			{
scheduledetailEditDto = new ScheduleDetailEditDto();
			}

			output.ScheduleDetail = scheduledetailEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改ScheduleDetail的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateScheduleDetail(CreateOrUpdateScheduleDetailInput input)
		{

			if (input.ScheduleDetail.Id.HasValue)
			{
				await UpdateScheduleDetailAsync(input.ScheduleDetail);
			}
			else
			{
				await CreateScheduleDetailAsync(input.ScheduleDetail);
			}
		}


		/// <summary>
		/// 新增ScheduleDetail
		/// </summary>
		[AbpAuthorize(ScheduleDetailAppPermissions.ScheduleDetail_Create)]
		protected virtual async Task<ScheduleDetailEditDto> CreateScheduleDetailAsync(ScheduleDetailEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <ScheduleDetail>(input);

			entity = await _scheduledetailRepository.InsertAsync(entity);
			return entity.MapTo<ScheduleDetailEditDto>();
		}

		/// <summary>
		/// 编辑ScheduleDetail
		/// </summary>
		[AbpAuthorize(ScheduleDetailAppPermissions.ScheduleDetail_Edit)]
		protected virtual async Task UpdateScheduleDetailAsync(ScheduleDetailEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _scheduledetailRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除ScheduleDetail信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(ScheduleDetailAppPermissions.ScheduleDetail_Delete)]
		public async Task DeleteScheduleDetail(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _scheduledetailRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除ScheduleDetail的方法
		/// </summary>
		          [AbpAuthorize(ScheduleDetailAppPermissions.ScheduleDetail_BatchDelete)]
		public async Task BatchDeleteScheduleDetailsAsync(List<Guid> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _scheduledetailRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出ScheduleDetail为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetScheduleDetailsToExcel()
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


