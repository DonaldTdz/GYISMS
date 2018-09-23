

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.ScheduleDetails;

namespace GYISMS.ScheduleDetails.Dtos
{
    public class ScheduleDetailEditDto:IHasCreationTime
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }


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
        public string EmployeeId { get; set; }


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
        public DateTime CreationTime { get; set; }

        public ScheduleStatusEnum Status { get; set; }

        [Required]
        public Guid ScheduleTaskId { get; set; }

        public string EmployeeName { get; set; }

        public string GrowerName { get; set; }
        public bool Checked { get; set; }
        public string AreaCode { get; set; }
    }
}