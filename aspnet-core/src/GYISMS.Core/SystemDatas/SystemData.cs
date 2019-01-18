using Abp.Domain.Entities;
using GYISMS.GYEnums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.SystemDatas
{
    /// <summary>
    /// 系统配置数据
    /// </summary>
    [Table("SystemDatas")]
    public class SystemData : Entity
    {

        /// <summary>
        /// 所属模块（会议管理、烟叶服务）
        /// </summary>
        public virtual ConfigModel? ModelId { get; set; }

        /// <summary>
        /// 配置类型（设备配置、会议物资、烟农单位、烟农村组、烟叶公共等）
        /// </summary>
        [Required]
        public virtual ConfigType Type { get; set; }

        /// <summary>
        /// 数据Code
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Code { get; set; }

        /// <summary>
        /// 配置描述
        /// </summary>
        [Required]
        [StringLength(500)]
        public virtual string Desc { get; set; }

        [StringLength(500)]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 序号（看需）
        /// </summary>
        public virtual int? Seq { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}
