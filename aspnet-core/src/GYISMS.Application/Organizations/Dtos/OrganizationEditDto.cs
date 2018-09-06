

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.Organizations;

namespace GYISMS.Organizations.Dtos
{
    public class OrganizationEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }


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






        //// custom codes

        //// custom codes end
    }
}