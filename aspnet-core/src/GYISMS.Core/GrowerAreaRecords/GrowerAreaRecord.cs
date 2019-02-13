using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.GrowerAreaRecords
{
    [Table("GrowerAreaRecords")]
    public class GrowerAreaRecord : Entity<Guid>
    {
        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public virtual int GrowerId { get; set; }

        /// <summary>
        /// 计划明细Id 外键
        /// </summary>
        public virtual Guid ScheduleDetailId { get; set; }

        /// <summary>
        /// 拍照图片
        /// </summary>
        [StringLength(500)]
        public virtual string ImgPath { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public virtual decimal? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public virtual decimal? Latitude { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        [StringLength(200)]
        public virtual string Location { get; set; }

        /// <summary>
        /// 采集人快照（烟技员名称）
        /// </summary>
        [StringLength(50)]
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// 采集人快照（烟技员Id）
        /// </summary>
        [StringLength(200)]
        public virtual string EmployeeId { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public virtual DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 采集面积
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public virtual string Remark { get; set; }


    }
}
