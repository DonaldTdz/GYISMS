

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.VisitExamines;

namespace  GYISMS.VisitExamines.Dtos
{
    public class VisitExamineEditDto
    {
/// <summary>
/// Id
/// </summary>
public Guid? Id { get; set; }


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