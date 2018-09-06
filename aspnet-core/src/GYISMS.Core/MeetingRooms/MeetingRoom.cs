using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.MeetingRooms
{
    /// <summary>
    /// 会议室
    /// </summary>
    [Table("MeetingRooms")]
    public class MeetingRoom : Entity<int>
    {
        /// <summary>
        /// 会议室名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 会议室图片
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Photo { get; set; }

        /// <summary>
        /// 坐席人数
        /// </summary>
        [Required]
        public virtual int Num { get; set; }

        /// <summary>
        /// 会议室类型（固定会议室、临时会议室）
        /// </summary>
        public virtual int? RoomType { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(500)]
        public virtual string Address { get; set; }

        /// <summary>
        /// 楼层信息（如：A栋1单元20层2003）
        /// </summary>
        public virtual string BuildDesc { get; set; }

        /// <summary>
        /// 是否需要审批
        /// </summary>
        public virtual bool? IsApprove { get; set; }

        /// <summary>
        /// 会议室管理员Id（需要审批 必须添加管理员）
        /// </summary>
        public virtual string ManagerId { get; set; }

        /// <summary>
        /// 会议室管理员名称
        /// </summary>
        [StringLength(50)]
        public virtual string ManagerName { get; set; }

        /// <summary>
        /// 第几排
        /// </summary>
        public virtual int? Row { get; set; }

        /// <summary>
        /// 第几号（从左到右边）
        /// </summary>
        public virtual int? Column { get; set; }

        /// <summary>
        /// 会议室布局（中心模式、矩阵模式）
        /// </summary>
        public virtual int? LayoutPattern { get; set; }

        /// <summary>
        /// 会议室平面图（平面图需提前发我们做处理）
        /// </summary>
        public virtual string PlanPath { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 会议室设备配置名称（逗号分隔）
        /// </summary>
        [StringLength(500)]
        public virtual string Devices { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool? IsDeleted { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }

        /// <summary>
        /// CreatorUserId
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// LastModificationTime
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// LastModifierUserId
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }

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
