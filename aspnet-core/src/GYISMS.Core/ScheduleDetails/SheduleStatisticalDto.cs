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
}
