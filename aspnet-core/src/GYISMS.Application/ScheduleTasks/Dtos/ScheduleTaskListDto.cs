

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleTasks;

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

        public string Thumb { get; set; }

        public string TaskName { get; set; }

        public DateTime? EndTime { get; set; }

        public string Extra { get; set; }

        public string Desc { get; set; }

        public int NumTotal { get; set; }

        public int CompleteNum { get; set; }

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