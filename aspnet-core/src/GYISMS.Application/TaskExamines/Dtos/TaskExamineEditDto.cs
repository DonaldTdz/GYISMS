

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.TaskExamines;

namespace  GYISMS.TaskExamines.Dtos
{
    public class TaskExamineEditDto
    {
/// <summary>
/// Id
/// </summary>
public int? Id { get; set; }


/// <summary>
/// TaskId
/// </summary>
public int? TaskId { get; set; }


/// <summary>
/// Name
/// </summary>
[Required(ErrorMessage="Name不能为空")]
public string Name { get; set; }


/// <summary>
/// Desc
/// </summary>
public string Desc { get; set; }


/// <summary>
/// Seq
/// </summary>
public int? Seq { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}