

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.VisitTasks;
using GYISMS.GYEnums;
using GYISMS.TaskExamines.Dtos;
using Abp.AutoMapper;

namespace GYISMS.VisitTasks.Dtos
{
    [AutoMapFrom(typeof(VisitTask))]
    public class VisitTaskListDto : EntityDto<int>
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

        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

        public Guid? ScheduleTaskId { get; set; }
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

        public bool Checked { get; set; }
        public int? VisitNum { get; set; }
        /// <summary>
        /// DeleterUserId
        /// </summary>
    }


}