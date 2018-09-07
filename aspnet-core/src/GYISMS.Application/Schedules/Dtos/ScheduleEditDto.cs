

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.Schedules;

namespace  GYISMS.Schedules.Dtos
{
    public class ScheduleEditDto
    {
/// <summary>
/// Id
/// </summary>
public Guid? Id { get; set; }


/// <summary>
/// Desc
/// </summary>
public string Desc { get; set; }


/// <summary>
/// Type
/// </summary>
[Required(ErrorMessage="Type不能为空")]
public int Type { get; set; }


/// <summary>
/// BeginTime
/// </summary>
public DateTime? BeginTime { get; set; }


/// <summary>
/// EndTime
/// </summary>
public DateTime? EndTime { get; set; }


/// <summary>
/// Status
/// </summary>
public int? Status { get; set; }


/// <summary>
/// PublishTime
/// </summary>
public DateTime? PublishTime { get; set; }


/// <summary>
/// IsDeleted
/// </summary>
public bool? IsDeleted { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }


/// <summary>
/// CreatorUserId
/// </summary>
public long? CreatorUserId { get; set; }


/// <summary>
/// LastModificationTime
/// </summary>
public DateTime? LastModificationTime { get; set; }


/// <summary>
/// LastModifierUserId
/// </summary>
public long? LastModifierUserId { get; set; }


/// <summary>
/// DeletionTime
/// </summary>
public DateTime? DeletionTime { get; set; }


/// <summary>
/// DeleterUserId
/// </summary>
public long? DeleterUserId { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}