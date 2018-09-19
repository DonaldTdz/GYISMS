using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GYISMS.TaskExamines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.VisitTasks
{
    /// <summary>
    /// 拜访任务
    /// </summary>
    [Table("VisitTasks")]
    public class VisitTask : AuditedEntity<int>
    {

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 任务类型（技术服务、生产管理、政策宣传、临时任务）
        /// </summary>
        [Required]
        public virtual int Type { get; set; }

        /// <summary>
        /// 是否需要考核
        /// </summary>
        public virtual bool? IsExamine { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 是否删除
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
