using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GYISMS.VisitTasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.TaskExamines
{
    /// <summary>
    /// 任务考核项
    /// </summary>
    [Table("TaskExamines")]
    public class TaskExamine : AuditedEntity<int>
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public virtual int? TaskId { get; set; }

        /// <summary>
        /// 考核项名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 考核项描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? Seq { get; set; }

        /// <summary>
        /// DeletionTime
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }

        /// <summary>
        /// DeleterUserId
        /// </summary>
        public virtual long? DeleterUserId { get; set; }
        public virtual bool? IsDeleted { get; set; }
    }
}
