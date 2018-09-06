using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.ScheduleTasks
{
    /// <summary>
    /// 计划任务
    /// </summary>
    [Table("ScheduleTasks")]
    public class ScheduleTask : Entity<Guid>
    {
        /// <summary>
        /// 任务Id 外键
        /// </summary>
        [Required]
        public virtual int TaskId { get; set; }

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
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}
