

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.VisitExamines;

namespace GYISMS.VisitExamines.Dtos
{
    public class VisitExamineListDto : EntityDto<Guid>
    {

/// <summary>
/// VisitRecordId
/// </summary>
public Guid? VisitRecordId { get; set; }


/// <summary>
/// EmployeeId
/// </summary>
public long? EmployeeId { get; set; }


/// <summary>
/// GrowerId
/// </summary>
public int? GrowerId { get; set; }


/// <summary>
/// TaskExamineId
/// </summary>
public int? TaskExamineId { get; set; }


/// <summary>
/// Score
/// </summary>
public int? Score { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}