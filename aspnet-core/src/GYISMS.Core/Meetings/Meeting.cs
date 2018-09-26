using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.Meetings
{
    /// <summary>
    /// 会议申请
    /// </summary>
    [Table("Meetings")]
    public class Meeting : AuditedEntity<Guid>, ISoftDelete
    {

        /// <summary>
        /// 会议室Id 外键
        /// </summary>
        [Required]
        public virtual int MeetingRoomId { get; set; }

        /// <summary>
        /// 会议主题
        /// </summary>
        [Required]
        [StringLength(100)]
        public virtual string Subject { get; set; }

        /// <summary>
        /// 会议议题，多个用逗号分隔
        /// </summary>
        [StringLength(2000)]
        public virtual string Issues { get; set; }

        /// <summary>
        /// 会议描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [Required]
        public virtual DateTime BeginTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [Required]
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// 主持人Id
        /// </summary>
        public virtual string HostId { get; set; }

        /// <summary>
        /// 主持人姓名
        /// </summary>
        public virtual string HostName { get; set; }

        /// <summary>
        /// 通知方式（发DING、钉钉消息）
        /// </summary>
        public virtual int? NoticeWay { get; set; }

        /// <summary>
        /// 提醒方式（无提醒、发DING、钉钉消息）
        /// </summary>
        public virtual int? RemindingWay { get; set; }

        /// <summary>
        /// 提醒时间提前（提前5分钟、提前10分钟、提前30分钟）
        /// </summary>
        public virtual int? RemindingTime { get; set; }

        /// <summary>
        /// 会议状态（提交审核、申请成功、取消）
        /// </summary>
        public virtual int? Status { get; set; }

        /// <summary>
        /// 审核人Id
        /// </summary>
        public virtual string AuditId { get; set; }

        /// <summary>
        /// 审核人名
        /// </summary>
        public virtual string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? AuditTime { get; set; }

        /// <summary>
        /// 取消会议用户Id
        /// </summary>
        public virtual string CancelUserId { get; set; }

        /// <summary>
        /// 取消用户名
        /// </summary>
        [StringLength(50)]
        public virtual string CancelUserName { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public virtual DateTime? CancelTime { get; set; }

        /// <summary>
        /// 会议负责人Id（默认创建人）
        /// </summary>
        public virtual string ResponsibleId { get; set; }

        /// <summary>
        /// 会议负责人名
        /// </summary>
        public virtual string ResponsibleName { get; set; }

        /// <summary>
        /// 是否需要指定座次 默认否
        /// </summary>
        public virtual bool? IsSeatingOrder { get; set; }

        /// <summary>
        /// 会议纪要，负责人才有权限
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// 资料回传的路径，多个已逗号分隔，负责人才有权限
        /// </summary>
        [StringLength(500)]
        public virtual string FilePath { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

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
