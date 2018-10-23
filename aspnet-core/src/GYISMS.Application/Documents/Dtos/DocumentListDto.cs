

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.Documents;
using Abp.AutoMapper;

namespace GYISMS.Documents.Dtos
{
    [AutoMapFrom(typeof(Document))]
    public class DocumentListDto : FullAuditedEntityDto<Guid> 
    {       
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// CategoryId
		/// </summary>
		public int CategoryId { get; set; }



		/// <summary>
		/// CategoryDesc
		/// </summary>
		public string CategoryDesc { get; set; }



		/// <summary>
		/// DeptIds
		/// </summary>
		public string DeptIds { get; set; }



		/// <summary>
		/// EmployeeIds
		/// </summary>
		public string EmployeeIds { get; set; }



		/// <summary>
		/// Summary
		/// </summary>
		public string Summary { get; set; }



		/// <summary>
		/// Content
		/// </summary>
		public string Content { get; set; }



		/// <summary>
		/// ReleaseDate
		/// </summary>
		public DateTime? ReleaseDate { get; set; }



		/// <summary>
		/// QrCodeUrl
		/// </summary>
		public string QrCodeUrl { get; set; }


        public string DeptDesc { get; set; }

        /// <summary>
        /// 授权员工名称（以逗号分隔）
        /// </summary>
        public string EmployeeDes { get; set; }

        public bool IsAllUser { get; set; }

    }
}