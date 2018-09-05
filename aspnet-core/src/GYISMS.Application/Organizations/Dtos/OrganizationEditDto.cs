

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.Organizations;

namespace  GYISMS.Organizations.Dtos
{
    public class OrganizationEditDto
    {
/// <summary>
/// Id
/// </summary>
public int? Id { get; set; }


/// <summary>
/// DepartmentName
/// </summary>
[Required(ErrorMessage="DepartmentName不能为空")]
public string DepartmentName { get; set; }


/// <summary>
/// ParentId
/// </summary>
[Required(ErrorMessage="ParentId不能为空")]
public int ParentId { get; set; }


/// <summary>
/// Order
/// </summary>
public int? Order { get; set; }


/// <summary>
/// DeptHiding
/// </summary>
public bool? DeptHiding { get; set; }


/// <summary>
/// OrgDeptOwner
/// </summary>
public string OrgDeptOwner { get; set; }


/// <summary>
/// IsDeleted
/// </summary>
[Required(ErrorMessage="IsDeleted不能为空")]
public bool IsDeleted { get; set; }


/// <summary>
/// CreationTime
/// </summary>
[Required(ErrorMessage="CreationTime不能为空")]
public DateTime CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}