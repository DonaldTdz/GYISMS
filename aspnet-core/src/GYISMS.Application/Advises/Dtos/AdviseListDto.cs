

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
		public Guid DocumentId { get; set; }



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

    //后台意见反馈dto
    public class AdviseDto : EntityDto<Guid>
    {
        public string DocumentName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string EmployeeName { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class DDAdviseDto
    {
        public string DocumentName { get; set; }

        /// <summary>
        /// EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }

        public string Content { get; set; }
        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }
        public string TimeFormat
        {
            get
            {
                return CreationTime.ToString("yyyy.MM.dd HH:mm");
            }
        }
    }
}