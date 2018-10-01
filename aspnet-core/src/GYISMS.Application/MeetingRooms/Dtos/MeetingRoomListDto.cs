

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.MeetingRooms;
using GYISMS.GYEnums;
using Abp.AutoMapper;

namespace GYISMS.MeetingRooms.Dtos
{
    [AutoMapFrom(typeof(MeetingRoom))]
    public class MeetingRoomListDto : EntityDto<int>
    {

        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


        /// <summary>
        /// Photo
        /// </summary>
        [Required(ErrorMessage = "Photo不能为空")]
        public string Photo { get; set; }


        /// <summary>
        /// Num
        /// </summary>
        [Required(ErrorMessage = "Num不能为空")]
        public int Num { get; set; }


        /// <summary>
        /// RoomType
        /// </summary>
        public RoomType RoomType { get; set; }

        public string TypeName
        {
            get
            {
                return RoomType.ToString();
            }
        }

        public string RoomTypeName { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// BuildDesc
        /// </summary>
        public string BuildDesc { get; set; }


        /// <summary>
        /// IsApprove
        /// </summary>
        public bool? IsApprove { get; set; }
        public string ApproveName { get; set; }

        /// <summary>
        /// ManagerId
        /// </summary>
        public long? ManagerId { get; set; }


        /// <summary>
        /// ManagerName
        /// </summary>
        public string ManagerName { get; set; }


        /// <summary>
        /// Row
        /// </summary>
        public int? Row { get; set; }


        /// <summary>
        /// Column
        /// </summary>
        public int? Column { get; set; }


        /// <summary>
        /// LayoutPattern
        /// </summary>
        public LayoutPattern? LayoutPattern { get; set; }
        public string LayoutName
        {
            get
            {
                return LayoutPattern.ToString();
            }
        }

        /// <summary>
        /// PlanPath
        /// </summary>
        public string PlanPath { get; set; }


        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// Devices
        /// </summary>
        public string Devices { get; set; }


        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }


        /// <summary>
        /// CreatorUserId
        /// </summary>
        public long? CreatorUserId { get; set; }


        /// <summary>
        /// LastModificationTime
        /// </summary>
        public DateTime? LastModificationTime { get; set; }


        /// <summary>
        /// LastModifierUserId
        /// </summary>
        public long? LastModifierUserId { get; set; }


        /// <summary>
        /// DeletionTime
        /// </summary>
        public DateTime? DeletionTime { get; set; }


        /// <summary>
        /// DeleterUserId
        /// </summary>
        public long? DeleterUserId { get; set; }

        public string CurrentStatus { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }


        //// custom codes

        //// custom codes end
    }

    public class MeetingStatus
    {
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
    }
}