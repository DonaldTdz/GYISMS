

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
        public AreaTypeEnum? Area { get; set; }

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
        public string SheduleName { get; set; }
    }
}
