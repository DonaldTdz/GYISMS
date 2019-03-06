
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GYISMS.Advises;

namespace  GYISMS.Advises.Dtos
{
    public class AdviseEditDto : Entity<Guid?>,IHasCreationTime
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
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