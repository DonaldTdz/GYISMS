

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using GYISMS.GrowerAreaRecords.Dtos;
using GYISMS.VisitRecords.Dtos;
using GYISMS.VisitExamines.Dtos;
using GYISMS.ScheduleDetails.Dtos;

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
        public AppUploadDto()
        {
            ScheduleDetail = new APPScheduleDetail();
        }

        public string EmployeeId { get; set; }
        //public List<ScheduleDetail> ScheduleDetailList { get; set; }
        //public List<Grower> GrowerList { get; set; }
        //public List<VisitExamine> VisitExamineList { get; set; }
        //public List<AppVisitRecord> VisitRecordList { get; set; }
        //public List<AppGrowerAreaRecord> GrowerAreaRecordList { get; set; }
        //public List<GrowerLocationLog> GrowerLocationLogList { get; set; }
        public APPScheduleDetail ScheduleDetail { get; set; }
        //public List<APPGrower> GrowerList { get; set; }
        public List<APPVisitExamine> VisitExamineList { get; set; }
        public List<AppVisitRecord> VisitRecordList { get; set; }
        public List<AppGrowerAreaRecord> GrowerAreaRecordList { get; set; }
        public List<APPGrowerLocationLog> GrowerLocationLogList { get; set; }
    }

    public class ImgBase64
    {
        public string imageBase64 { get; set; }
    }
}
