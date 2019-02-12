

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.Schedules;

namespace GYISMS.Schedules.Dtos
{
    [AutoMapTo(typeof(Schedule))]
    public class ScheduleEditDto : FullAuditedEntityDto<Guid?>
    {

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


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

    }
}