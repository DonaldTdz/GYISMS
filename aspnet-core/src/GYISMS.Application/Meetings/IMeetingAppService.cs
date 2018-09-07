

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.Meetings.Dtos;
using GYISMS.Meetings;

namespace GYISMS.Meetings
{
    /// <summary>
    /// Meeting应用层服务的接口方法
    ///</summary>
    public interface IMeetingAppService : IApplicationService
    {
        /// <summary>
    /// 获取Meeting的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingListDto>> GetPagedMeetings(GetMeetingsInput input);

		/// <summary>
		/// 通过指定id获取MeetingListDto信息
		/// </summary>
		Task<MeetingListDto> GetMeetingByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出Meeting为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetMeetingsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMeetingForEditOutput> GetMeetingForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetMeetingForEditOutput


        /// <summary>
        /// 添加或者修改Meeting的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateMeeting(CreateOrUpdateMeetingInput input);


        /// <summary>
        /// 删除Meeting信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMeeting(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Meeting
        /// </summary>
        Task BatchDeleteMeetingsAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
