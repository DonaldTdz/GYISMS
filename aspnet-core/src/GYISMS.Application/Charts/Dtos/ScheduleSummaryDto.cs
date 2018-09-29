using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Charts.Dtos
{
    public class ScheduleSummaryDto
    {
        public ScheduleSummaryDto()
        {
            A = "1";
        }
        /// <summary>
        /// 统计名称 完成、进行中、逾期
        /// </summary>
        public string Name { get; set; }

        public int? Num { get; set; }

        public decimal? Percent { get; set; }

        public string A { get; set; }

        public string ClassName { get; set; }

        public int Seq { get; set; }
    }
}
