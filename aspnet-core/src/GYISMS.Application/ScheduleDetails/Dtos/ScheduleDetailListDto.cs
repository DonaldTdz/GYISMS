

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleDetails;

namespace GYISMS.ScheduleDetails.Dtos
{
    public class ScheduleDetailListDto : EntityDto<Guid>
    {

        /// <summary>
        /// TaskId
        /// </summary>
        [Required(ErrorMessage = "TaskId不能为空")]
        public int TaskId { get; set; }


        /// <summary>
        /// ScheduleId
        /// </summary>
        [Required(ErrorMessage = "ScheduleId不能为空")]
        public Guid ScheduleId { get; set; }


        /// <summary>
        /// EmployeeId
        /// </summary>
        [Required(ErrorMessage = "EmployeeId不能为空")]
        public long EmployeeId { get; set; }


        /// <summary>
        /// GrowerId
        /// </summary>
        [Required(ErrorMessage = "GrowerId不能为空")]
        public int GrowerId { get; set; }


        /// <summary>
        /// VisitNum
        /// </summary>
        public int? VisitNum { get; set; }


        /// <summary>
        /// CompleteNum
        /// </summary>
        public int? CompleteNum { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }


        public int? Status { get; set; }
    }
}