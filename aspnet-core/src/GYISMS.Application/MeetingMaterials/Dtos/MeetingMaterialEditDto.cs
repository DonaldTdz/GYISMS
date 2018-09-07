

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.MeetingMaterials;

namespace  GYISMS.MeetingMaterials.Dtos
{
    public class MeetingMaterialEditDto
    {
/// <summary>
/// Id
/// </summary>
public Guid? Id { get; set; }


/// <summary>
/// MeetingId
/// </summary>
[Required(ErrorMessage="MeetingId不能为空")]
public Guid MeetingId { get; set; }


/// <summary>
/// Code
/// </summary>
public string Code { get; set; }


/// <summary>
/// Name
/// </summary>
[Required(ErrorMessage="Name不能为空")]
public string Name { get; set; }


/// <summary>
/// Num
/// </summary>
public int? Num { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}