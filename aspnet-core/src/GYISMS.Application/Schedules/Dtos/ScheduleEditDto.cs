

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.Schedules;

namespace GYISMS.Schedules.Dtos
{
    public class ScheduleEditDto : AuditedEntityDto<Guid?>
    {

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public int Type { get; set; }


        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime? BeginTime { get; set; }


        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        public ScheduleMasterStatusEnum Status { get; set; }


        /// <summary>
        /// PublishTime
        /// </summary>
        public DateTime? PublishTime { get; set; }


        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }


        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }


        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }






        //// custom codes

        //// custom codes end
    }
}