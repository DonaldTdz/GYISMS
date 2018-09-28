

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using GYISMS.ScheduleDetails;
using System;

namespace GYISMS.ScheduleDetails.Dtos
{
    public class GetScheduleDetailsInput : PagedAndSortedInputDto, IShouldNormalize
    {
          /// <summary>
		 /// 模糊搜索使用的关键字
		 ///</summary>
        public string Filter { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public AreaTypeEnum? AreaCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// 烟农
        /// </summary>
        public string GrowerName { get; set; }

        /// <summary>
        /// 烟技员
        /// </summary>
        public string EmployeeName { get; set; }

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

    public class ScheduleDetaStatisticalInput
    {
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }

    }

    public class SheduleSumInput
    {
        /// <summary>
        /// 区域
        /// </summary>
        public AreaTypeEnum? AreaCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public int? TaskId { get; set; }
    }
}
