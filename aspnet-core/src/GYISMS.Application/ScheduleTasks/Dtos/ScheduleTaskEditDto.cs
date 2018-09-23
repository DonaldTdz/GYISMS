

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.ScheduleTasks;

namespace GYISMS.ScheduleTasks.Dtos
{
    public class ScheduleTaskEditDto : AuditedEntityDto<Guid?>
    {
        /// <summary>
        /// TaskId
        /// </summary>
        [Required(ErrorMessage = "TaskId不能为空")]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        /// <summary>
        /// ScheduleId
        /// </summary>
        [Required(ErrorMessage = "ScheduleId不能为空")]
        public Guid ScheduleId { get; set; }


        /// <summary>
        /// VisitNum
        /// </summary>
        public int? VisitNum { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }


        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        //public DateTime? CreationTime { get; set; }

        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }
    }
}