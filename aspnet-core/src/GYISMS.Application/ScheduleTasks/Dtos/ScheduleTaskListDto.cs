

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleTasks;
using GYISMS.GYEnums;
using GYISMS.Growers.Dtos;
using Abp.AutoMapper;
using GYISMS.VisitRecords;

namespace GYISMS.ScheduleTasks.Dtos
{
    public class ScheduleTaskListDto : EntityDto<Guid>
    {

        /// <summary>
        /// TaskId
        /// </summary>
        [Required(ErrorMessage = "TaskId不能为空")]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        /// <summary>
        /// ScheduleId
        /// </summary>
        [Required(ErrorMessage = "ScheduleId不能为空")]
        public Guid ScheduleId { get; set; }


        /// <summary>
        /// VisitNum
        /// </summary>
        public int? VisitNum { get; set; }

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


        public string TypeName{ get; set; }

        public bool? IsExamine { get; set; }
    }

    public class DingDingScheduleTaskDto
    {
        public Guid Id { get; set; }

        public string Thumb
        {
            get
            {
                if (EndDay <= 1)
                {
                    return "../../image/warn.png";
                }

                if (EndDay < 5)
                {
                    return "../../image/warn_y.png";
                }
                return "../../image/icon-tasknor.png";
            }
        }

        public TaskTypeEnum TaskType { get; set; }

        public string TaskName { get; set; }

        public DateTime? EndTime { get; set; }

        public string Extra
        {
            get
            {
                return string.Format("剩余{0}天", EndDay);
            }
        }

        public string Desc
        {
            get
            {
                return string.Format("已拜访{0}次共{1}次", CompleteNum, NumTotal);
            }
        }

        public string Title
        {
            get
            {
                return TaskName + "（" + TaskType.ToString() + "）";
            }
        }

        public int? NumTotal { get; set; }

        public int? CompleteNum { get; set; }

        public int EndDay
        {
            get
            {
                if (EndTime.HasValue)
                {
                    return (EndTime - DateTime.Today).Value.Days;
                }
                return 0;
            }
        }

        public string EndTimeFormat
        {
            get
            {
                return EndTime.HasValue ? EndTime.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }
    }

    public class DingDingTaskDto
    {
        public DingDingTaskDto()
        {
            Growers = new List<DingDingTaskGrowerDto>();
        }

        public Guid Id { get; set; }//ScheduleTaskId

        public string TaskNam { get; set; }

        public TaskTypeEnum TaskType { get; set; }

        public string TaskTitle
        {
            get
            {
                return string.Format("{0}（{1}）", TaskNam, TaskType.ToString());
            }
        }

        public string ScheduleTitle { get; set; }

        public string StatusDesc
        {
            get
            {
                if (CompleteNum == VisitTotal)
                {
                    return "已完成";
                }

                if (EndDay < 0)
                {
                    return "已逾期";
                }

                if (CompleteNum > 0)
                {
                    return "进行中";
                }

                return "未开始";
            }
        }

        public string StatusImg
        {
            get
            {
                if (CompleteNum == VisitTotal)
                {
                    return "../../../image/icon_status-dot-G.png";
                }

                if (EndDay < 0)
                {
                    return "../../../image/icon_status-dot-R1.png";
                }

                if (EndDay == 1)
                {
                    return "../../../image/icon_status-dot-R.png";
                }

                if (EndDay < 5)
                {
                    return "../../../image/icon_status-dot-Y.png";
                }
                return "../../../image/icon_status-dot-GR.png";
            }
        }

        public int VisitTotal { get; set; }

        public int CompleteNum { get; set; }

        public int Percent
        {
            get
            {
                if (VisitTotal > 0)
                {
                    return (CompleteNum / VisitTotal * 100);
                }
                return 0;
            }
        }

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

        public int EndDay
        {
            get
            {
                if (EndTime.HasValue)
                {
                    return (EndTime - DateTime.Today).Value.Days;
                }
                return 0;
            }
        }

        public List<DingDingTaskGrowerDto> Growers{ get; set; }

    }

    public class DingDingTaskGrowerDto
    {
        public Guid Id { get; set; }//ScheduleDetailId

        public string GrowerName { get; set; }

        public string UnitName { get; set; }

        public int? VisitNum { get; set; }

        public int? CompleteNum { get; set; }
    }

    public class DingDingVisitGrowerDetailDto
    {
        public Guid Id { get; set; }//ScheduleDetailId

        public string TaskNam { get; set; }

        public TaskTypeEnum TaskType { get; set; }

        public string TaskTitle
        {
            get
            {
                return string.Format("{0}（{1}）", TaskNam, TaskType.ToString());
            }
        }

        public int? VisitNum { get; set; }

        public int? CompleteNum { get; set; }

        public int? GrowerId { get; set; }

        public string Footer
        {
            get
            {
                return string.Format("已完成{0}次拜访共{1}次拜访", CompleteNum, VisitNum);
            }
        }

        public ScheduleStatusEnum ScheduleStatus { get; set; }

        public GrowerListDto GrowerInfo { get; set; }

        public List<DingDingVisitRecordDto> VisitRecords { get; set; }
    }

    [AutoMapFrom(typeof(VisitRecord))]
    public class DingDingVisitRecordDto
    {
        public Guid Id { get; set; }

        public string Location { get; set; }

        public DateTime? CreationTime { get; set; }

        public string CreationTimeFormat
        {
            get
            {
                if (CreationTime.HasValue)
                {
                    return CreationTime.Value.ToString("MM-dd HH:mm");
                }
                return string.Empty;
            }
        }
    }
}