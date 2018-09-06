

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Organizations;

namespace GYISMS.Organizations.Dtos
{
    public class OrganizationListDto : EntityDto<long>
    {

        /// <summary>
        /// DepartmentName
        /// </summary>
        [Required(ErrorMessage = "DepartmentName不能为空")]
        public string DepartmentName { get; set; }


        /// <summary>
        /// ParentId
        /// </summary>
        public long? ParentId { get; set; }


        /// <summary>
        /// Order
        /// </summary>
        public long? Order { get; set; }


        /// <summary>
        /// DeptHiding
        /// </summary>
        public bool? DeptHiding { get; set; }


        /// <summary>
        /// OrgDeptOwner
        /// </summary>
        public string OrgDeptOwner { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }
    }

    public class NzTreeNode
    {
        public string title { get; set; }
        public string key { get; set; }
        public NzTreeNode[] children {get;set;}
    }
}