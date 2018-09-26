using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.VisitExamines
{
    /// <summary>
    /// 拜访考核
    /// </summary>
    [Table("VisitExamines")]
    public class VisitExamine : Entity<Guid>, IHasCreationTime
    {
        public VisitExamine()
        {
            CreationTime = DateTime.Now;
        }

        /// <summary>
        /// 拜访记录Id 外键
        /// </summary>
        public virtual Guid? VisitRecordId { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        public virtual string EmployeeId { get; set; }

        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public virtual int? GrowerId { get; set; }

        /// <summary>
        /// 考核项Id 外键
        /// </summary>
        public virtual int? TaskExamineId { get; set; }

        /// <summary>
        /// 考核得分（优得5分、良得3分、差得1分）
        /// </summary>
        public virtual int? Score { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
