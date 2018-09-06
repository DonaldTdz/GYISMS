

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleTasks;

namespace GYISMS.ScheduleTasks.Dtos
{
    public class ScheduleTaskListDto : EntityDto<Guid>
    {

/// <summary>
/// TaskId
/// </summary>
[Required(ErrorMessage="TaskId不能为空")]
public int TaskId { get; set; }


/// <summary>
/// ScheduleId
/// </summary>
[Required(ErrorMessage="ScheduleId不能为空")]
public Guid ScheduleId { get; set; }


/// <summary>
/// VisitNum
/// </summary>
public int? VisitNum { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}