

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.SystemDatas;

namespace GYISMS.SystemDatas.Dtos
{
    public class SystemDataListDto : EntityDto<int>
    {

/// <summary>
/// ModelId
/// </summary>
public int? ModelId { get; set; }


/// <summary>
/// Type
/// </summary>
[Required(ErrorMessage="Type不能为空")]
public int Type { get; set; }


/// <summary>
/// Code
/// </summary>
[Required(ErrorMessage="Code不能为空")]
public string Code { get; set; }


/// <summary>
/// Desc
/// </summary>
[Required(ErrorMessage="Desc不能为空")]
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