

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.VisitRecords;
using Abp.AutoMapper;
using GYISMS.TaskExamines;
using GYISMS.VisitExamines;

namespace GYISMS.VisitRecords.Dtos
{
    public class VisitRecordListDto : EntityDto<Guid>
    {

        /// <summary>
        /// ScheduleDetailId
        /// </summary>
        [Required(ErrorMessage = "ScheduleDetailId不能为空")]
        public Guid ScheduleDetailId { get; set; }


        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        /// <summary>
        /// GrowerId
        /// </summary>
        public int? GrowerId { get; set; }


        /// <summary>
        /// SignTime
        /// </summary>
        public DateTime? SignTime { get; set; }


        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }


        /// <summary>
        /// Longitude
        /// </summary>
        public decimal? Longitude { get; set; }


        /// <summary>
        /// Latitude
        /// </summary>
        public decimal? Latitude { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// ImgPath
        /// </summary>
        public string ImgPath { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }






        //// custom codes

        //// custom codes end
    }


    [AutoMapTo(typeof(VisitRecord))]
    [AutoMapFrom(typeof(VisitRecord))]
    public class DingDingVisitRecordInputDto
    {
        public Guid ScheduleDetailId { get; set; }

        public string Location { get; set; }

        public decimal? Longitude { get; set; }


        public decimal? Latitude { get; set; }

        public string Desc { get; set; }

        public string ImgPath { get; set; }

        public int? GrowerId { get; set; }

        public string GrowerName { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeImg { get; set; }

        public DateTime? SignTime { get; set; }

        public string TaskDesc { get; set; }

        public string SignTimeFormat
        {
            get
            {
                return SignTime.Value.ToString("HH:mm yyyy.MM.dd");
            }
        }

        public string SummDesc
        {
            get
            {
                return string.Format("{0}拜访{1} {2}", TaskDesc, GrowerName, Desc);
            }
        }

        public List<DingDingTaskExamineDto> Examines { get; set; }
    }

    [AutoMapFrom(typeof(TaskExamine))]
    public class DingDingTaskExamineDto : EntityDto
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public int? Score { get; set; }

        public string ScoreName
        {
            get
            {
                switch (Score)
                {
                    case 5: return "优";
                    case 3: return "良";
                    case 1: return "差";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}