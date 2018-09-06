

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
        public virtual string title { get; set; }
        public virtual string key { get; set; }

        public virtual bool expanded { get; set; }//是否打开

        public virtual bool isLeaf { get; set; } //是否是树叶

        public virtual List<NzTreeNode> children { get; set; }
    }

    public class OrganizationNzTreeNode : NzTreeNode
    {
        public override bool expanded
        {
            get
            {
                if (key == "1")//总公司
                {
                    return true;
                }
                return false;
            }
        }

        public override bool isLeaf
        {
            get
            {
                if (children.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public new List<OrganizationNzTreeNode> children { get; set; }
    }
}