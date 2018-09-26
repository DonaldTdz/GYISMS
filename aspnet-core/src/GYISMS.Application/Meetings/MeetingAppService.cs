
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

using GYISMS.Meetings.Authorization;
using GYISMS.Meetings.Dtos;
using GYISMS.Meetings;
using GYISMS.Authorization;

namespace GYISMS.Meetings
{
    /// <summary>
    /// Meeting应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class MeetingAppService : GYISMSAppServiceBase, IMeetingAppService
    {
        private readonly IRepository<Meeting, Guid> _meetingRepository;
        private readonly IMeetingManager _meetingManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public MeetingAppService(IRepository<Meeting, Guid> meetingRepository
            , IMeetingManager meetingManager
            )
        {
            _meetingRepository = meetingRepository;
            _meetingManager = meetingManager;
        }


        /// <summary>
        /// 获取Meeting的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingListDto>> GetPagedMeetings(GetMeetingsInput input)
        {

            var query = _meetingRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var meetingCount = await query.CountAsync();

            var meetings = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var meetingListDtos = ObjectMapper.Map<List <MeetingListDto>>(meetings);
            var meetingListDtos = meetings.MapTo<List<MeetingListDto>>();

            return new PagedResultDto<MeetingListDto>(
meetingCount,
meetingListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取MeetingListDto信息
        /// </summary>
        public async Task<MeetingListDto> GetMeetingByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _meetingRepository.GetAsync(input.Id);

            return entity.MapTo<MeetingListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetMeetingForEditOutput> GetMeetingForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetMeetingForEditOutput();
            MeetingEditDto meetingEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _meetingRepository.GetAsync(input.Id.Value);

                meetingEditDto = entity.MapTo<MeetingEditDto>();

                //meetingEditDto = ObjectMapper.Map<List <meetingEditDto>>(entity);
            }
            else
            {
                meetingEditDto = new MeetingEditDto();
            }

            output.Meeting = meetingEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Meeting的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MeetingEditDto> CreateOrUpdateMeeting(MeetingEditDto input)
        {

            if (input.Id.HasValue)
            {
                return await UpdateMeetingAsync(input);
            }
            else
            {
                return await CreateMeetingAsync(input);
            }
        }


        /// <summary>
        /// 新增Meeting
        /// </summary>
        [AbpAuthorize(MeetingAppPermissions.Meeting_Create)]
        protected virtual async Task<MeetingEditDto> CreateMeetingAsync(MeetingEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Meeting>(input);

            entity = await _meetingRepository.InsertAsync(entity);
            return entity.MapTo<MeetingEditDto>();
        }

        /// <summary>
        /// 编辑Meeting
        /// </summary>
        protected virtual async Task<MeetingEditDto> UpdateMeetingAsync(MeetingEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _meetingRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            var result = await _meetingRepository.UpdateAsync(entity);
            return result.MapTo<MeetingEditDto>();
        }



        /// <summary>
        /// 删除Meeting信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(MeetingAppPermissions.Meeting_Delete)]
        public async Task DeleteMeeting(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _meetingRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Meeting的方法
        /// </summary>
        [AbpAuthorize(MeetingAppPermissions.Meeting_BatchDelete)]
        public async Task BatchDeleteMeetingsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _meetingRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出Meeting为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetMeetingsToExcel()
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


