using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.Growers
{
    /// <summary>
    /// 烟农
    /// </summary
    [Table("Growers")]
    public class Grower : AuditedEntity<int>, ISoftDelete
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
        public virtual int? CountyCode { get; set; }

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
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        //public virtual DateTime? CreationTime { get; set; }

        ///// <summary>
        ///// CreatorUserId
        ///// </summary>
        //public virtual long? CreatorUserId { get; set; }

        ///// <summary>
        ///// LastModificationTime
        ///// </summary>
        //public virtual DateTime? LastModificationTime { get; set; }

        ///// <summary>
        ///// LastModifierUserId
        ///// </summary>
        //public virtual long? LastModifierUserId { get; set; }

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
