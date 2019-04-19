

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.VisitRecords;
using Abp.AutoMapper;
using GYISMS.TaskExamines;
using GYISMS.VisitExamines;
using GYISMS.GYEnums;
using Abp.Domain.Entities;

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
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string ExaminesName { get; set; }
        public bool? HasExamine { get; set; }

        public string ImgTop
        {
            get
            {
                if (ImgPaths.Length > 0)
                {
                    return ImgPaths[0];
                }
                return string.Empty;
            }
        }

        public string[] ImgPaths
        {
            get
            {
                if (!string.IsNullOrEmpty(ImgPath))
                {
                    return ImgPath.Split(',');
                }
                return new string[0];
            }
        }
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
                if (SignTime.HasValue)
                {
                    return SignTime.Value.ToString("HH:mm yyyy.MM.dd");
                }
                return string.Empty;
            }
        }

        public string SummDesc
        {
            get
            {
                return string.Format("{0}拜访{1}", TaskDesc, GrowerName);
            }
        }

        public List<DingDingTaskExamineDto> Examines { get; set; }

        public string ImgTop
        {
            get
            {
                if (ImgPaths.Length > 0)
                {
                    return ImgPaths[0];
                }
                return string.Empty;
            }
        }

        public string[] ImgPaths
        {
            get
            {
                if (!string.IsNullOrEmpty(ImgPath))
                {
                    return ImgPath.Split(',');
                }
                return new string[0];
            }
        }
    }

    public class DingDingAreaRecordInputDto : DingDingVisitRecordInputDto
    {
        /// <summary>
        /// 落实面积
        /// </summary>
        public decimal? Area { get; set; }

        public ScheduleStatusEnum? ScheduleStatus { get; set; }
    }

    [AutoMapFrom(typeof(TaskExamine))]
    public class DingDingTaskExamineDto : EntityDto
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public int? Score { get; set; }

        public ExamineOptionEnum ExamineOption { get; set; }

        public string ScoreName
        {
            get
            {
                switch (ExamineOption)
                {
                    case ExamineOptionEnum.优差等级:
                        {
                            switch (Score)
                            {
                                case 5: return "优";
                                case 3: return "合格";
                                case 1: return "差";
                                default:
                                    return string.Empty;
                            }
                        }
                    case ExamineOptionEnum.到位情况:
                        {
                            if (Score == 5)
                            {
                                return "到位";
                            }
                            return "不到位";
                        }
                    case ExamineOptionEnum.了解情况:
                        {
                            if (Score == 5)
                            {
                                return "了解";
                            }
                            return "不了解";
                        }
                    default:
                        return string.Empty;
                }

              
            }
        }
    }

    [AutoMapFrom(typeof(VisitRecord))]
    public class AppVisitRecord : Entity<Guid>
    {

        /// <summary>
        /// 任务明细Id 外键
        /// </summary>
        public Guid ScheduleDetailId { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public int? GrowerId { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime? SignTime { get; set; }

        /// <summary>
        /// 签到地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 签到经度
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// 签到纬度
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 签到说明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 生成签到拍照生成水印图片路径
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}