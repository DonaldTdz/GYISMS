using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.MeetingParticipants
{
    /// <summary>
    /// 参会人员
    /// </summary>
    [Table("MeetingParticipants")]
    public class MeetingParticipant : Entity<Guid>
    {

        /// <summary>
        /// 会议Id
        /// </summary>
        [Required]
        public virtual Guid MeetingId { get; set; }

        /// <summary>
        /// 参会人员Id
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// 参会人员名
        /// </summary>
        [Required]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 座次第几排
        /// </summary>
        public virtual int? Row { get; set; }

        /// <summary>
        /// 座次第几号（从左到右）
        /// </summary>
        public virtual int? Column { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public virtual DateTime? ConfirmTime { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public virtual DateTime? SignTime { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}
