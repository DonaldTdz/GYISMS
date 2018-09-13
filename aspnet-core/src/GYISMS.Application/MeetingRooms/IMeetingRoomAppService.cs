

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.MeetingRooms.Dtos;
using GYISMS.MeetingRooms;

namespace GYISMS.MeetingRooms
{
    /// <summary>
    /// MeetingRoom应用层服务的接口方法
    ///</summary>
    public interface IMeetingRoomAppService : IApplicationService
    {
        /// <summary>
    /// 获取MeetingRoom的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingRoomListDto>> GetPagedMeetingRoomsAsync(GetMeetingRoomsInput input);


        /// <summary>
        /// 导出MeetingRoom为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetMeetingRoomsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMeetingRoomForEditOutput> GetMeetingRoomForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetMeetingRoomForEditOutput


        /// <summary>
        /// 添加或者修改MeetingRoom的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateMeetingRoom(CreateOrUpdateMeetingRoomInput input);


        /// <summary>
        /// 删除MeetingRoom信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMeetingRoom(EntityDto<int> input);


        /// <summary>
        /// 批量删除MeetingRoom
        /// </summary>
        Task BatchDeleteMeetingRoomsAsync(List<int> input);
        Task<MeetingRoomEditDto> CreateOrUpdateMeetingRoomAsycn(MeetingRoomEditDto input);
        Task<MeetingRoomListDto> GetMeetingRoomByIdAsync(int id);
        Task MeetingRoomDeleteByIdAsync(MeetingRoomEditDto input);
    }
}
