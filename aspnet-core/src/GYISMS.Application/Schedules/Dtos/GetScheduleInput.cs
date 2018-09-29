

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using GYISMS.Schedules;
using System;

namespace GYISMS.Schedules.Dtos
{
    public class GetSchedulesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        ///</summary>
        public string Name { get; set; }
        public Guid ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public ScheduleType? ScheduleType {get;set;}
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
}
