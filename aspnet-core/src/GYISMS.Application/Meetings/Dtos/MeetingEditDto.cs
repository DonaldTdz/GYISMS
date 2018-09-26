

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using GYISMS.Meetings;

namespace GYISMS.Meetings.Dtos
{
    public class MeetingEditDto : AuditedEntityDto<Guid?>
    {

        /// <summary>
        /// MeetingRoomId
        /// </summary>
        [Required(ErrorMessage = "MeetingRoomId不能为空")]
        public int MeetingRoomId { get; set; }


        /// <summary>
        /// Subject
        /// </summary>
        [Required(ErrorMessage = "Subject不能为空")]
        public string Subject { get; set; }


        /// <summary>
        /// Issues
        /// </summary>
        public string Issues { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// BeginTime
        /// </summary>
        [Required(ErrorMessage = "BeginTime不能为空")]
        public DateTime BeginTime { get; set; }


        /// <summary>
        /// EndTime
        /// </summary>
        [Required(ErrorMessage = "EndTime不能为空")]
        public DateTime EndTime { get; set; }


        /// <summary>
        /// HostId
        /// </summary>
        public long? HostId { get; set; }


        /// <summary>
        /// HostName
        /// </summary>
        public string HostName { get; set; }


        /// <summary>
        /// NoticeWay
        /// </summary>
        public int? NoticeWay { get; set; }


        /// <summary>
        /// RemindingWay
        /// </summary>
        public int? RemindingWay { get; set; }


        /// <summary>
        /// RemindingTime
        /// </summary>
        public int? RemindingTime { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }


        /// <summary>
        /// AuditId
        /// </summary>
        public long? AuditId { get; set; }


        /// <summary>
        /// AuditName
        /// </summary>
        public string AuditName { get; set; }


        /// <summary>
        /// AuditTime
        /// </summary>
        public DateTime? AuditTime { get; set; }


        /// <summary>
        /// CancelUserId
        /// </summary>
        public long? CancelUserId { get; set; }


        /// <summary>
        /// CancelUserName
        /// </summary>
        public string CancelUserName { get; set; }


        /// <summary>
        /// CancelTime
        /// </summary>
        public DateTime? CancelTime { get; set; }


        /// <summary>
        /// ResponsibleId
        /// </summary>
        public long? ResponsibleId { get; set; }


        /// <summary>
        /// ResponsibleName
        /// </summary>
        public string ResponsibleName { get; set; }


        /// <summary>
        /// IsSeatingOrder
        /// </summary>
        public bool? IsSeatingOrder { get; set; }


        /// <summary>
        /// Summary
        /// </summary>
        public string Summary { get; set; }


        /// <summary>
        /// FilePath
        /// </summary>
        public string FilePath { get; set; }


        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }


        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }


        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }
    }
}