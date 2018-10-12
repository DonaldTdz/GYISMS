using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Charts.Dtos
{
   public class GetSheduleDetailInput
    {
        /// <summary>
        /// 区域
        /// </summary>
        public AreaCodeEnum? AreaCode { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskTypeEnum? TaskType { get; set; }
        public int? TaskId { get; set; }
        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 任务状态(总体-完成计算的是计划中的任务全部完成 1进行中，2完成，0逾期)
        /// </summary>
        public int? TStatus { get; set; }

        /// <summary>
        /// 任务状态（完成计算的是计划中有完成的数包括计划中没有全部完成的和全部完成的1计划，2完成，0逾期 ）
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 烟技员
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 烟农
        /// </summary>
        public string GrowerName { get; set; }

        public int PageIndex { get; set; }
    }
}
