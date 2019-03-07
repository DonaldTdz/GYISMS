using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.Advises
{
    [Table("Advises")]
    public class Advise : Entity<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual Guid DocumentId { get; set; }

        /// <summary>
        /// 员工id
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名快照
        /// </summary>
        [StringLength(20)]
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(500)]
        public virtual string Content { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
