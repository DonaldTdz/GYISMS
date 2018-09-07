
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

using GYISMS.MeetingRooms.Authorization;
using GYISMS.MeetingRooms.Dtos;
using GYISMS.MeetingRooms;
using GYISMS.Authorization;

namespace GYISMS.MeetingRooms
{
    /// <summary>
    /// MeetingRoom应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class MeetingRoomAppService : GYISMSAppServiceBase, IMeetingRoomAppService
    {
    private readonly IRepository<MeetingRoom, int>
    _meetingroomRepository;
    
       
       private readonly IMeetingRoomManager _meetingroomManager;

    /// <summary>
        /// 构造函数 
        ///</summary>
    public MeetingRoomAppService(
    IRepository<MeetingRoom, int>
meetingroomRepository
        ,IMeetingRoomManager meetingroomManager
        )
        {
        _meetingroomRepository = meetingroomRepository;
  _meetingroomManager=meetingroomManager;
        }


        /// <summary>
            /// 获取MeetingRoom的分页列表信息
            ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  async  Task<PagedResultDto<MeetingRoomListDto>> GetPagedMeetingRooms(GetMeetingRoomsInput input)
		{

		    var query = _meetingroomRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件

			var meetingroomCount = await query.CountAsync();

			var meetingrooms = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

				// var meetingroomListDtos = ObjectMapper.Map<List <MeetingRoomListDto>>(meetingrooms);
				var meetingroomListDtos =meetingrooms.MapTo<List<MeetingRoomListDto>>();

				return new PagedResultDto<MeetingRoomListDto>(
meetingroomCount,
meetingroomListDtos
					);
		}


		/// <summary>
		/// 通过指定id获取MeetingRoomListDto信息
		/// </summary>
		public async Task<MeetingRoomListDto> GetMeetingRoomByIdAsync(EntityDto<int> input)
		{
			var entity = await _meetingroomRepository.GetAsync(input.Id);

		    return entity.MapTo<MeetingRoomListDto>();
		}

		/// <summary>
		/// MPA版本才会用到的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async  Task<GetMeetingRoomForEditOutput> GetMeetingRoomForEdit(NullableIdDto<int> input)
		{
			var output = new GetMeetingRoomForEditOutput();
MeetingRoomEditDto meetingroomEditDto;

			if (input.Id.HasValue)
			{
				var entity = await _meetingroomRepository.GetAsync(input.Id.Value);

meetingroomEditDto = entity.MapTo<MeetingRoomEditDto>();

				//meetingroomEditDto = ObjectMapper.Map<List <meetingroomEditDto>>(entity);
			}
			else
			{
meetingroomEditDto = new MeetingRoomEditDto();
			}

			output.MeetingRoom = meetingroomEditDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改MeetingRoom的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateMeetingRoom(CreateOrUpdateMeetingRoomInput input)
		{

			if (input.MeetingRoom.Id.HasValue)
			{
				await UpdateMeetingRoomAsync(input.MeetingRoom);
			}
			else
			{
				await CreateMeetingRoomAsync(input.MeetingRoom);
			}
		}


		/// <summary>
		/// 新增MeetingRoom
		/// </summary>
		[AbpAuthorize(MeetingRoomAppPermissions.MeetingRoom_Create)]
		protected virtual async Task<MeetingRoomEditDto> CreateMeetingRoomAsync(MeetingRoomEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

			var entity = ObjectMapper.Map <MeetingRoom>(input);

			entity = await _meetingroomRepository.InsertAsync(entity);
			return entity.MapTo<MeetingRoomEditDto>();
		}

		/// <summary>
		/// 编辑MeetingRoom
		/// </summary>
		[AbpAuthorize(MeetingRoomAppPermissions.MeetingRoom_Edit)]
		protected virtual async Task UpdateMeetingRoomAsync(MeetingRoomEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _meetingroomRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _meetingroomRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除MeetingRoom信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(MeetingRoomAppPermissions.MeetingRoom_Delete)]
		public async Task DeleteMeetingRoom(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _meetingroomRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除MeetingRoom的方法
		/// </summary>
		          [AbpAuthorize(MeetingRoomAppPermissions.MeetingRoom_BatchDelete)]
		public async Task BatchDeleteMeetingRoomsAsync(List<int> input)
		{
			//TODO:批量删除前的逻辑判断，是否允许删除
			await _meetingroomRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出MeetingRoom为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetMeetingRoomsToExcel()
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


