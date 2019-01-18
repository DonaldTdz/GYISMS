using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GYISMS.Schedules
{
    /// <summary>
    /// 计划表
    /// </summary>
    [Table("Schedules")]
    public class Schedule : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 计划说明
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 计划类型（每月、每周、每日、自定义）
        /// </summary>
        [Required]
        public virtual ScheduleType Type { get; set; }

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
        public virtual ScheduleMasterStatusEnum Status { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime? PublishTime { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }
    }
}
