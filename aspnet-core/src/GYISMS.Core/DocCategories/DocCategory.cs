using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.DocCategories
{
    [Table("DocCategories")]
    public class DocCategory : FullAuditedEntity
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 父Id（root 为 0）
        /// </summary>
        public virtual int? ParentId { get; set; }
        /// <summary>
        /// 类别描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }

        /// <summary>
        /// 维护部门Id
        /// </summary>
        public virtual long? DeptId { get; set; }
    }
}
