using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GYISMS.Schedules
{
    /// <summary>
    /// 计划表
    /// </summary>
    [Table("Schedules")]
    public class Schedule : AuditedEntity<Guid>
    {
        /// <summary>
        /// 计划说明
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 计划类型（每月、每周、每日）
        /// </summary>
        [Required]
        public virtual int Type { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public virtual DateTime? BeginTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// 计划状态（草稿、已发布）
        /// </summary>
        public virtual int? Status { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime? PublishTime { get; set; }

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
