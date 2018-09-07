using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.MeetingMaterials
{
    /// <summary>
    /// 会议物资
    /// </summary>
    [Table("MeetingMaterials")]
    public class MeetingMaterial : Entity<Guid>
    {

        /// <summary>
        /// 会议Id 外键
        /// </summary>
        [Required]
        public virtual Guid MeetingId { get; set; }

        /// <summary>
        /// 物资Code 从系统数据表取
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 物资名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public virtual int? Num { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}
