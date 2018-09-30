using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Charts.Dtos
{
    public class DistrictDto
    {
        public AreaTypeEnum? AreaType { get; set; }

        public string District
        {
            get
            {
                if (AreaType.HasValue)
                {
                    return AreaType.ToString();
                }
                return string.Empty;
            }
        }

        public int? VisitNum { get; set; }

        public int? CompleteNum { get; set; }

        public int? OverdueNum { get; set; }

        public decimal? Percent
        {
            get
            {
                if (VisitNum.HasValue && VisitNum > 0)
                {
                    return Math.Round(CompleteNum.Value / (decimal)VisitNum.Value, 2) * 100;
                }
                return 0;
            }
        }
    }

    public class DistrictChartItemDto
    {
        public string Name { get; set; }

        public string District { get; set; }

        public int? Num { get; set; }
    }

    public class DistrictChartDto
    {
        public DistrictChartDto()
        {
            Districts = new List<DistrictDto>();

        }

        public List<DistrictDto> Districts { get; set; }

        public List<DistrictChartItemDto> Items
        {
            get
            {
                var items = new List<DistrictChartItemDto>();
                foreach (var item in Districts)
                {
                    items.Add(new DistrictChartItemDto()
                    {
                        District = item.District,
                        Name = "计划",
                        Num = item.VisitNum
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        District = item.District,
                        Name = "完成",
                        Num = item.CompleteNum
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        District = item.District,
                        Name = "逾期",
                        Num = item.OverdueNum
                    });
                }
                return items;
            }
        }
    }
}
