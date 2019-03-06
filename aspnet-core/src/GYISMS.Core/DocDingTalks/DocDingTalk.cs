using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GYISMS.DocDingTalks
{
    [Table("DocDingTalks")]
    public class DocDingTalk : Entity<Guid>, IHasCreationTime
    {
        public DocDingTalk()
        {
            CreationTime = DateTime.Now;
        }

        /// <summary>
        /// 文档附件Id
        /// </summary>
        [Required]
        public virtual Guid DocAttId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        [StringLength(50)]
        public virtual string UserId { get; set; }

        /// <summary>
        /// 钉盘空间Id
        /// </summary>
        [StringLength(50)]
        public virtual string SpaceId { get; set; }

        /// <summary>
        /// 钉盘文件Id
        /// </summary>
        [StringLength(50)]
        public virtual string FileId { get; set; }

        /// <summary>
        /// 钉盘文件名
        /// </summary>
        [StringLength(50)]
        public virtual string FileName { get; set; }

        /// <summary>
        /// 钉盘文件扩展名
        /// </summary>
        [StringLength(50)]
        public virtual string FileType { get; set; }

        /// <summary>
        /// 钉盘文件大小
        /// </summary>
        public virtual long? FileSize { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

    }
}
