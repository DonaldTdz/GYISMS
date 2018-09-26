using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.VisitRecords
{
    /// <summary>
    /// 拜访记录
    /// </summary>
    [Table("VisitRecords")]
    public class VisitRecord : Entity<Guid>, IHasCreationTime
    {
        public VisitRecord()
        {
            CreationTime = DateTime.Now;
        }

        /// <summary>
        /// 任务明细Id 外键
        /// </summary>
        [Required]
        public virtual Guid ScheduleDetailId { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        public virtual string EmployeeId { get; set; }

        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public virtual int? GrowerId { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public virtual DateTime? SignTime { get; set; }

        /// <summary>
        /// 签到地点
        /// </summary>
        [StringLength(200)]
        public virtual string Location { get; set; }

        /// <summary>
        /// 签到经度
        /// </summary>
        public virtual decimal? Longitude { get; set; }

        /// <summary>
        /// 签到纬度
        /// </summary>
        public virtual decimal? Latitude { get; set; }

        /// <summary>
        /// 签到说明
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 生成签到拍照生成水印图片路径
        /// </summary>
        [StringLength(200)]
        public virtual string ImgPath { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
