

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleTasks;
using GYISMS.GYEnums;

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
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }

        //// custom codes

        //// custom codes end
    }

    public class DingDingScheduleTaskDto
    {
        public Guid Id { get; set; }

        public string Thumb
        {
            get
            {
                if (EndDay == 1)
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
    }
}