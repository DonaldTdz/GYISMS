

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.Growers;
using GYISMS.GYEnums;
using System;

namespace GYISMS.Growers.Dtos
{
    public class GetGrowersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        ///</summary>
        public string Name { get; set; }
        public int TaskId { get; set; }
        public Guid ScheduleId { get; set; }
        public string Employee { get; set; }
        public Guid? Id { get; set; }
        public int VisitNum { get; set; }
        public Guid ScheduleTaskId { get; set; }
        public string EmployeeId { get; set; }
        public AreaTypeEnum? Area { get; set; }
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
