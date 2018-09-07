

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.MeetingParticipants;

namespace GYISMS.MeetingParticipants.Dtos
{
    public class MeetingParticipantListDto : EntityDto<Guid>
    {

/// <summary>
/// MeetingId
/// </summary>
[Required(ErrorMessage="MeetingId不能为空")]
public Guid MeetingId { get; set; }


/// <summary>
/// UserId
/// </summary>
public string UserId { get; set; }


/// <summary>
/// UserName
/// </summary>
[Required(ErrorMessage="UserName不能为空")]
public string UserName { get; set; }


/// <summary>
/// Row
/// </summary>
public int? Row { get; set; }


/// <summary>
/// Column
/// </summary>
public int? Column { get; set; }


/// <summary>
/// ConfirmTime
/// </summary>
public DateTime? ConfirmTime { get; set; }


/// <summary>
/// SignTime
/// </summary>
public DateTime? SignTime { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}