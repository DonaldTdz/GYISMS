

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Schedules;
using GYISMS.GYEnums;

namespace GYISMS.Schedules.Dtos
{
    public class ScheduleListDto : EntityDto<Guid>
    {

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public ScheduleType Type { get; set; }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

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
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }


        /// <summary>
        /// CreatorUserId
        /// </summary>
        public long? CreatorUserId { get; set; }


        /// <summary>
        /// LastModificationTime
        /// </summary>
        public DateTime? LastModificationTime { get; set; }


        /// <summary>
        /// LastModifierUserId
        /// </summary>
        public long? LastModifierUserId { get; set; }


        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }


        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }
    }
}