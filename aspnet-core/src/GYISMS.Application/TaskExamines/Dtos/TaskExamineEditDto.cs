

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.TaskExamines;

namespace GYISMS.TaskExamines.Dtos
{
    public class TaskExamineEditDto : FullAuditedEntityDto
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

        public virtual ExamineOptionEnum ExamineOption { get; set; }

        public string ExamineOptionDesc
        {
            get
            {
                switch (ExamineOption)
                {
                    case ExamineOptionEnum.优差等级:
                        return "优/合格/差";
                    case ExamineOptionEnum.到位情况:
                        return "到位/不到位";
                    case ExamineOptionEnum.了解情况:
                        return "了解/不了解";
                    default:
                        break;
                }
                return string.Empty;
            }
        }
    }
}