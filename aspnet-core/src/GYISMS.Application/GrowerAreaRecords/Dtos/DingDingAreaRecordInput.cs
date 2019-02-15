using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.GrowerAreaRecords.Dtos
{
    public class DingDingAreaRecordInput
    {
        public Guid ScheduleDetailId { get; set; }

        public string[] ImgPaths { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public string Location { get; set; }

        public decimal? Area { get; set; }


    }
}
