

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.Advises;

namespace GYISMS.Advises.Dtos
{
    public class AdviseListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// DocumentId
		/// </summary>
		[Required(ErrorMessage="DocumentId不能为空")]
		public string DocumentId { get; set; }



		/// <summary>
		/// EmployeeId
		/// </summary>
		[Required(ErrorMessage="EmployeeId不能为空")]
		public string EmployeeId { get; set; }



		/// <summary>
		/// EmployeeName
		/// </summary>
		public string EmployeeName { get; set; }



		/// <summary>
		/// Content
		/// </summary>
		public string Content { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }




    }
}