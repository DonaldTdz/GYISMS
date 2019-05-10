using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using GYISMS.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.Growers
{
    /// <summary>
    /// 烟农
    /// </summary
    [Table("Growers")]
    public class Grower : FullAuditedEntity, ISoftDelete, IMayArea
    {
        /// <summary>
        /// 业务年度
        /// </summary>
        public virtual int? Year { get; set; }

        /// <summary>
        /// 单位code 从系统数据表取
        /// </summary>
        [StringLength(20)]
        public virtual string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [StringLength(50)]
        public virtual string UnitName { get; set; }

        /// <summary>
        /// 烟农名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 所属区县（剑阁县、昭化区、旺苍县）
        /// </summary>
        public virtual AreaCodeEnum? AreaCode { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        [StringLength(200)]
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 烟技员姓名(快照)
        /// </summary>
        [StringLength(200)]
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [StringLength(50)]
        public virtual string ContractNo { get; set; }

        /// <summary>
        /// 村组 从系统数据表取
        /// </summary>
        [StringLength(50)]
        public virtual string VillageGroup { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(20)]
        public virtual string Tel { get; set; }

        /// <summary>
        /// 详细住址
        /// </summary>
        [StringLength(500)]
        public virtual string Address { get; set; }

        /// <summary>
        /// 种植主体类型（普通烟农）
        /// </summary>
        public virtual int? Type { get; set; }

        /// <summary>
        /// 种植面积（单位：亩）
        /// </summary>
        public virtual decimal? PlantingArea { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public virtual decimal? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public virtual decimal? Latitude { get; set; }

        /// <summary>
        /// 合同签订时间
        /// </summary>
        public virtual DateTime? ContractTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }

        /// <summary>
        /// 采集次数
        /// </summary>
        public virtual int CollectNum { get; set; }

        /// <summary>
        ///预计单产（单位：担/亩）2019-2-13
        /// </summary>
        public virtual decimal? UnitVolume { get; set; }

        /// <summary>
        /// 核实面积 2019-2-13
        /// </summary>
        public virtual decimal? ActualArea { get; set; }

        /// <summary>
        /// 核实面积状态（枚举：未核实 0， 已核实 1）2019-2-13
        /// </summary>
        public virtual AreaStatusEnum? AreaStatus { get; set; }

        /// <summary>
        /// 核实面积时间 2019-2-13
        /// </summary>
        public virtual DateTime? AreaTime { get; set; }

        /// <summary>
        /// 采用核实面积的计划明细Id 2019-2-15
        /// </summary>
        public virtual Guid? AreaScheduleDetailId { get; set; }

    }
}
