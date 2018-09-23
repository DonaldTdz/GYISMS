using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.ScheduleTasks
{
    /// <summary>
    /// 计划任务
    /// </summary>
    [Table("ScheduleTasks")]
    public class ScheduleTask : AuditedEntity<Guid>
    {
        /// <summary>
        /// 任务Id 外键
        /// </summary>
        [Required]
        public virtual int TaskId { get; set; }
        public virtual string TaskName { get; set; }
        /// <summary>
        /// 计划Id 外键
        /// </summary>
        [Required]
        public virtual Guid ScheduleId { get; set; }

        /// <summary>
        /// 默认拜访次数，在生成明细里后可以更改
        /// </summary>
        public virtual int? VisitNum { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>
        public virtual bool? IsDeleted { get; set; }

        /// <summary>
        /// DeletionTime
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }

        /// <summary>
        /// DeleterUserId
        /// </summary>
        public virtual long? DeleterUserId { get; set; }
    }
}
