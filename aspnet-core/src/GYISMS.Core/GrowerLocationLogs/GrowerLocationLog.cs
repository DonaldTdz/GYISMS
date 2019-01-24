using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.GrowerLocationLogs
{
    /// <summary>
    /// 烟农地理位置修改日志
    /// </summary>
    [Table("GrowerLocationLogs")]
    public class GrowerLocationLog: Entity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        //public Guid Id { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [StringLength(200)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// 烟农
        /// </summary>
        [Required]
        public int GrowerId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
      
    }
}
