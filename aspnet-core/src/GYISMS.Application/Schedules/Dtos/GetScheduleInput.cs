

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using GYISMS.Schedules;
using System;
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
using System.Collections.Generic;

namespace GYISMS.Schedules.Dtos
{
    public class GetSchedulesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        ///</summary>
        public string Name { get; set; }
        public Guid ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public ScheduleType? ScheduleType {get;set;}
        //// custom codes

        //// custom codes end

        /// <summary>
        /// 正常化排序使用
        ///</summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }


    }

    public class AppUploadDto {
        public string EmployeeId { get; set; }
        //public List<Schedule> ScheduleList { get; set; }
        public List<ScheduleDetail> ScheduleDetailList { get; set; }
        //public List<ScheduleTask> ScheduleTaskList { get; set; }
        public List<Grower> GrowerList { get; set; }
        //public List<TaskExamine> TaskExamineList { get; set; }
        public List<VisitExamine> VisitExamineList { get; set; }
        public List<VisitRecord> VisitRecordList { get; set; }
        //public List<VisitTask> VisitTaskList { get; set; }
        public List<GrowerAreaRecord> GrowerAreaRecordList { get; set; }
        public List<GrowerLocationLog> GrowerLocationLogList { get; set; }
    }

    public class ImgBase64
    {
        public string imageBase64 { get; set; }
    }
}
