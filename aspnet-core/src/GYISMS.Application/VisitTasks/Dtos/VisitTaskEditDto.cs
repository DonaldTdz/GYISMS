

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.TaskExamines.Dtos;
using GYISMS.VisitTasks;

namespace GYISMS.VisitTasks.Dtos
{
    public class VisitTaskEditDto : AuditedEntityDto<int?>
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public TaskTypeEnum Type { get; set; }


        /// <summary>
        /// IsExamine
        /// </summary>
        public bool? IsExamine { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


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

        public List<TaskExamineEditDto> TaskExamineList { get; set; }
    }
}