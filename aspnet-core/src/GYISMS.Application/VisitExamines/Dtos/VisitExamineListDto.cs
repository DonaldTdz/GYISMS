

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.VisitExamines;
using Abp.Domain.Entities;
using Abp.AutoMapper;
using GYISMS.VisitRecords;

namespace GYISMS.VisitExamines.Dtos
{
    public class VisitExamineListDto : EntityDto<Guid>
    {

/// <summary>
/// VisitRecordId
/// </summary>
public Guid? VisitRecordId { get; set; }


/// <summary>
/// EmployeeId
/// </summary>
public long? EmployeeId { get; set; }


/// <summary>
/// GrowerId
/// </summary>
public int? GrowerId { get; set; }


/// <summary>
/// TaskExamineId
/// </summary>
public int? TaskExamineId { get; set; }


/// <summary>
/// Score
/// </summary>
public int? Score { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }

    [AutoMapFrom(typeof(VisitRecord))]

    public class APPVisitExamine : Entity<Guid>
    {

        /// <summary>
        /// 拜访记录Id 外键
        /// </summary>
        public Guid? VisitRecordId { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        public  string EmployeeId { get; set; }

        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public  int? GrowerId { get; set; }

        /// <summary>
        /// 考核项Id 外键
        /// </summary>
        public  int? TaskExamineId { get; set; }

        /// <summary>
        /// 考核得分（优得5分、良得3分、差得1分）
        /// </summary>
        public  int? Score { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}