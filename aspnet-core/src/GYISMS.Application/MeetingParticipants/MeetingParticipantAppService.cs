
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

using GYISMS.MeetingParticipants.Authorization;
using GYISMS.MeetingParticipants.Dtos;
using GYISMS.MeetingParticipants;
using GYISMS.Authorization;

namespace GYISMS.MeetingParticipants
{
    /// <summary>
    /// MeetingParticipant应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class MeetingParticipantAppService : GYISMSAppServiceBase, IMeetingParticipantAppService
    {
    private readonly IRepository<MeetingParticipant, Guid>
    _meetingparticipantRepository;
    
       
       private readonly IMeetingParticipantManager _meetingparticipantManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public MeetingParticipantAppService(
    IRepository<MeetingParticipant, Guid>
meetingparticipantRepository
        ,IMeetingParticipantManager meetingparticipantManager
        )
        {
        _meetingparticipantRepository = meetingparticipantRepository;
  _meetingparticipantManager=meetingparticipantManager;
        }


        /// <summary>
            /// 获取MeetingParticipant的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<MeetingParticipantListDto>> GetPagedMeetingParticipants(GetMeetingParticipantsInput input)
		{

		    var query = _meetingparticipantRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var meetingparticipantCount = await query.CountAsync();

			var meetingparticipants = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var meetingparticipantListDtos = ObjectMapper.Map<List <MeetingParticipantListDto>>(meetingparticipants);
				var meetingparticipantListDtos =meetingparticipants.MapTo<List<MeetingParticipantListDto>>();

				return new PagedResultDto<MeetingParticipantListDto>(
meetingparticipantCount,
meetingparticipantListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取MeetingParticipantListDto信息
		/// </summary>
		public async Task<MeetingParticipantListDto> GetMeetingParticipantByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _meetingparticipantRepository.GetAsync(input.Id);

		    return entity.MapTo<MeetingParticipantListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetMeetingParticipantForEditOutput> GetMeetingParticipantForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetMeetingParticipantForEditOutput();
MeetingParticipantEditDto meetingparticipantEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _meetingparticipantRepository.GetAsync(input.Id.Value);

meetingparticipantEditDto = entity.MapTo<MeetingParticipantEditDto>();

				//meetingparticipantEditDto = ObjectMapper.Map<List <meetingparticipantEditDto>>(entity);
			}
			else
			{
meetingparticipantEditDto = new MeetingParticipantEditDto();
			}

			output.MeetingParticipant = meetingparticipantEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改MeetingParticipant的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateMeetingParticipant(CreateOrUpdateMeetingParticipantInput input)
		{

			if (input.MeetingParticipant.Id.HasValue)
			{
				await UpdateMeetingParticipantAsync(input.MeetingParticipant);
			}
			else
			{
				await CreateMeetingParticipantAsync(input.MeetingParticipant);
			}
		}


		/// <summary>
		/// 新增MeetingParticipant
		/// </summary>
		[AbpAuthorize(MeetingParticipantAppPermissions.MeetingParticipant_Create)]
		protected virtual async Task<MeetingParticipantEditDto> CreateMeetingParticipantAsync(MeetingParticipantEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <MeetingParticipant>(input);

			entity = await _meetingparticipantRepository.InsertAsync(entity);
			return entity.MapTo<MeetingParticipantEditDto>();
		}

		/// <summary>
		/// 编辑MeetingParticipant
		/// </summary>
		[AbpAuthorize(MeetingParticipantAppPermissions.MeetingParticipant_Edit)]
		protected virtual async Task UpdateMeetingParticipantAsync(MeetingParticipantEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _meetingparticipantRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _meetingparticipantRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除MeetingParticipant信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(MeetingParticipantAppPermissions.MeetingParticipant_Delete)]
		public async Task DeleteMeetingParticipant(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _meetingparticipantRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除MeetingParticipant的方法
		/// </summary>
		          [AbpAuthorize(MeetingParticipantAppPermissions.MeetingParticipant_BatchDelete)]
		public async Task BatchDeleteMeetingParticipantsAsync(List<Guid> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _meetingparticipantRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出MeetingParticipant为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetMeetingParticipantsToExcel()
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


