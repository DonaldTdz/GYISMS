

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Schedules;
using GYISMS.GYEnums;
using Abp.AutoMapper;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleTasks.Dtos;
using GYISMS.Growers.Dtos;
using GYISMS.TaskExamines.Dtos;
using GYISMS.VisitExamines.Dtos;
using GYISMS.VisitRecords.Dtos;
using GYISMS.VisitTasks.Dtos;
using GYISMS.GrowerAreaRecords.Dtos;
using GYISMS.GrowerLocationLogs;
using GYISMS.GrowerAreaRecords;
using GYISMS.VisitTasks;
using GYISMS.VisitRecords;
using GYISMS.VisitExamines;
using GYISMS.TaskExamines;
using GYISMS.Growers;
using GYISMS.ScheduleTasks;
using GYISMS.ScheduleDetails;
using GYISMS.SystemDatas;

namespace GYISMS.Schedules.Dtos
{
    [AutoMapFrom(typeof(Schedule))]
    public class ScheduleListDto : EntityDto<Guid>
    {

        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public ScheduleType Type { get; set; }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime? BeginTime { get; set; }

        public string BeginTimeFormat
        {
            get
            {
                if (BeginTime.HasValue)
                {
                    return BeginTime.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; }

        public string EndTimeFormat
        {
            get
            {
                if (EndTime.HasValue)
                {
                    return EndTime.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// Status
        /// </summary>
        public ScheduleMasterStatusEnum Status { get; set; }


        /// <summary>
        /// PublishTime
        /// </summary>
        public DateTime? PublishTime { get; set; }


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

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }
        public string CreateUserName { get; set; }
        public string Area { get; set; }

        public int? CompleteCount { get; set; }

        public int? VisitCount { get; set; }
        public int Percentage
        {
            get
            {
                if (VisitCount > 0)
                {
                    return (int)((CompleteCount.Value / ((decimal)VisitCount.Value)) * 100);
                }
                return 0;
            }
        }
    }

    public class AppSyncData
    {
        public AppSyncData()
        {
            ScheduleList = new List<Schedule>();
            ScheduleDetailList = new List<ScheduleDetail>();
            ScheduleTaskList = new List<ScheduleTask>();
            GrowerList = new List<Grower>();
            TaskExamineList = new List<TaskExamine>();
            VisitExamineList = new List<VisitExamine>();
            VisitRecordList = new List<VisitRecord>();
            VisitTaskList = new List<VisitTask>();
            GrowerAreaRecordList = new List<GrowerAreaRecord>();
            GrowerLocationLogList = new List<GrowerLocationLog>();
            SystemDataList = new List<SystemData>();
        }
        public List<Schedule> ScheduleList { get; set; }
        public List<ScheduleDetail> ScheduleDetailList { get; set; }

        public List<ScheduleTask> ScheduleTaskList { get; set; }
        public List<Grower> GrowerList { get; set; }
        public List<TaskExamine> TaskExamineList { get; set; }
        public List<VisitExamine> VisitExamineList { get; set; }
        public List<VisitRecord> VisitRecordList { get; set; }
        public List<VisitTask> VisitTaskList { get; set; }
        public List<GrowerAreaRecord> GrowerAreaRecordList { get; set; }
        public List<GrowerLocationLog> GrowerLocationLogList { get; set; }
        public List<SystemData> SystemDataList { get; set; }
        //public AppSyncData()
        //{
        //    List<ScheduleListDto> ScheduleList = new List<ScheduleListDto>();
        //    List<ScheduleDetailListDto> ScheduleDetialList = new List<ScheduleDetailListDto>();
        //    List<ScheduleTaskListDto> ScheduleTaskList = new List<ScheduleTaskListDto>();
        //    List<GrowerListDto> GrowerList = new List<GrowerListDto>();
        //    List<TaskExamineListDto> TaskExamineList = new List<TaskExamineListDto>();
        //    List<VisitExamineListDto> VisitExamineList = new List<VisitExamineListDto>();
        //    List<VisitRecordListDto> VisitRecordList = new List<VisitRecordListDto>();
        //    List<VisitTaskListDto> VisitTaskList = new List<VisitTaskListDto>();
        //    List<GrowerAreaRecordListDto> GrowerAreaRecordList = new List<GrowerAreaRecordListDto>();
        //    List<GrowerLocationLog> GrowerLocationLogList = new List<GrowerLocationLog>();
        //}
        //public List<ScheduleListDto> ScheduleList { get; set; }
        //public List<ScheduleDetailListDto> ScheduleDetialList { get; set; }

        //public List<ScheduleTaskListDto> ScheduleTaskList { get; set; }
        //public List<GrowerListDto> GrowerList { get; set; }
        //public List<TaskExamineListDto> TaskExamineList { get; set; }
        //public List<VisitExamineListDto> VisitExamineList { get; set; }
        //public List<VisitRecordListDto> VisitRecordList { get; set; }
        //public List<VisitTaskListDto> VisitTaskList { get; set; }
        //public List<GrowerAreaRecordListDto> GrowerAreaRecordList { get; set; }
        //public List<GrowerLocationLog> GrowerLocationLogList { get; set; }
    }
}