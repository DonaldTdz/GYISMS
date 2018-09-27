using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.ScheduleDetails
{
   public class SheduleStatisticalDto
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// 已完成数
        /// </summary>
        public int? Completed { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? Expired { get; set; }
    }

    public class SheduleSumDto
    {
        /// <summary>
        /// 区域
        /// </summary>
        public AreaTypeEnum? AreaCode { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskTypeEnum TaskType { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// 已完成数
        /// </summary>
        public int? Complete { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? Expired { get; set; }

        /// <summary>
        /// 计划开始
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 区域名
        /// </summary>
        public string AreaName
        {
            get
            {
                return AreaCode.ToString();
            }
        }

        public string TaskTypeName
        {
            get
            {
                return TaskType.ToString();
            }
        }

        public string CompleteRate
        {
            get
            {
                if (Complete == 0)
                {
                    return "0%";
                }
                else
                {
                    //return (Math.Round((double)Complete / Total, 2) * 100).ToString() + "%";
                    return "0%";
                }
            }
        }
    }
}
