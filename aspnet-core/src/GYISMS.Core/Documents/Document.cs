using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.Documents
{
    [Table("Documents")]
    public class Document : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 资料名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 外键资料分类Id
        /// </summary>
        public virtual int CategoryId { get; set; }
        /// <summary>
        ///分类名描述（分类名层级以逗号分隔）
        /// </summary>
        [StringLength(500)]
        public virtual string CategoryDesc { get; set; }
        /// <summary>
        /// 部门Ids(以逗号分隔)
        /// </summary>
        [StringLength(500)]
        public virtual string DeptIds { get; set; }
        /// <summary>
        /// 员工授权Ids（以逗号分隔）
        /// </summary>
        public virtual string EmployeeIds { get; set; }
        /// <summary>
        /// 摘要说明
        /// </summary>
        [StringLength(2000)]
        public virtual string Summary { get; set; }
        /// <summary>
        /// 文档内容 支持HTML
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        public virtual DateTime? ReleaseDate { get; set; }
        /// <summary>
        /// 二维码URL
        /// </summary>
        [StringLength(200)]
        public virtual string QrCodeUrl { get; set; }
        /// <summary>
        /// 授权部门名称（以逗号分隔）
        /// </summary>
        [StringLength(1000)]
        public virtual string DeptDesc { get; set; }
        /// <summary>
        /// 授权员工名称（以逗号分隔）
        /// </summary>
        public virtual string EmployeeDes { get; set; }
    }
}
