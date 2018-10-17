using Abp.Domain.Entities.Auditing;
using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.DocAttachments
{
    [Table("DocAttachments")]
    public class DocAttachment : FullAuditedEntity
    {
        /// <summary>
        /// 附件名
        /// </summary>
        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 枚举（PDF、Word、Excel）
        /// </summary>
        public virtual FileTypeEnum? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual decimal? FileSize { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        [StringLength(500)]
        public virtual string Path { get; set; }
        /// <summary>
        /// 外键 资料表Id
        /// </summary>
        public virtual Guid DocId { get; set; }

    }
}
