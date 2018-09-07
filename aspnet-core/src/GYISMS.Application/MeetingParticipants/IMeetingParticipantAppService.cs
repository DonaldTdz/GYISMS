

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.MeetingParticipants.Dtos;
using GYISMS.MeetingParticipants;

namespace GYISMS.MeetingParticipants
{
    /// <summary>
    /// MeetingParticipant应用层服务的接口方法
    ///</summary>
    public interface IMeetingParticipantAppService : IApplicationService
    {
        /// <summary>
    /// 获取MeetingParticipant的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingParticipantListDto>> GetPagedMeetingParticipants(GetMeetingParticipantsInput input);

		/// <summary>
		/// 通过指定id获取MeetingParticipantListDto信息
		/// </summary>
		Task<MeetingParticipantListDto> GetMeetingParticipantByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出MeetingParticipant为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetMeetingParticipantsToExcel();

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMeetingParticipantForEditOutput> GetMeetingParticipantForEdit(NullableIdDto<Guid> input);

        //todo:缺少Dto的生成GetMeetingParticipantForEditOutput


        /// <summary>
        /// 添加或者修改MeetingParticipant的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateMeetingParticipant(CreateOrUpdateMeetingParticipantInput input);


        /// <summary>
        /// 删除MeetingParticipant信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMeetingParticipant(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除MeetingParticipant
        /// </summary>
        Task BatchDeleteMeetingParticipantsAsync(List<Guid> input);


		//// custom codes
		 
        //// custom codes end
    }
}
