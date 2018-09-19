

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.TaskExamines;

namespace GYISMS.TaskExamines.Dtos
{
    public class TaskExamineEditDto : AuditedEntityDto
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public int? TaskId { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// Seq
        /// </summary>
        public int? Seq { get; set; }

        public bool? IsDeleted { get; set; }
        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }


        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }
    }
}