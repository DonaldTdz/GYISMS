

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.GrowerAreaRecords;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace GYISMS.GrowerAreaRecords.Dtos
{
    [AutoMapFrom(typeof(GrowerAreaRecord))]
    public class GrowerAreaRecordListDto : EntityDto<Guid> 
    {

        
		/// <summary>
		/// GrowerId
		/// </summary>
		public int GrowerId { get; set; }



		/// <summary>
		/// ImgPath
		/// </summary>
		public string ImgPath { get; set; }



		/// <summary>
		/// Longitude
		/// </summary>
		public decimal? Longitude { get; set; }



		/// <summary>
		/// Latitude
		/// </summary>
		public decimal? Latitude { get; set; }



		/// <summary>
		/// Location
		/// </summary>
		public string Location { get; set; }



		/// <summary>
		/// EmployeeName
		/// </summary>
		public string EmployeeName { get; set; }



		/// <summary>
		/// EmployeeId
		/// </summary>
		public string EmployeeId { get; set; }



		/// <summary>
		/// CollectionTime
		/// </summary>
		public DateTime? CollectionTime { get; set; }



		/// <summary>
		/// Area
		/// </summary>
		public decimal? Area { get; set; }



		/// <summary>
		/// Remark
		/// </summary>
		public string Remark { get; set; }

        /// <summary>
        /// 计划明细Id 外键
        /// </summary>
        public Guid ScheduleDetailId { get; set; }

        public string ScheduleName { get; set; }
    }

    /// <summary>
    /// 市级统计图
    /// </summary>
    public class CityAreaChartDto
    {
        public List<AreaChartDto> list;
        public decimal Actual;
        public decimal Expected;
    }

    /// <summary>
    /// 区县统计图
    /// </summary>
    public class DistrictAreaChartDto
    {
        public List<AreaChartDto> list;
        public decimal ZhActual;
        public decimal ZhExpected;
        public decimal JgActual;
        public decimal JgExpected;
        public decimal WcActual;
        public decimal WcExpected;
    }

    /// <summary>
    /// 图表dto
    /// </summary>
    public class AreaChartDto
    {
        public string AreaName { get; set; }
        public decimal Area { get; set; }
        public string GroupName { get; set; }
    }

    /// <summary>
    /// 详情dto
    /// </summary>
    public class AreaDetailDto
    {
        public decimal Actual { get; set; }
        public decimal Expected { get; set; }
    }
    //public class AreaDingDingCharts
    //{
    //    public AreaDingDingCharts()
    //    {
    //        AreaChart = new List<AreaChartDto>();
    //    }

    //    public List<AreaChartDto> AreaChart { get; set; }

    //    public List<AreaChartDto> Items
    //    {
    //        get
    //        {
    //            var items = new List<AreaChartDto>();
    //            foreach (var item in AreaChart)
    //            {
    //                items.Add(new AreaChartDto()
    //                {
    //                    GroupName = item.GroupName,

    //                });
    //                items.Add(new AreaChartDto()
    //                {

    //                });
    //            }
    //            return items;
    //        }
    //    }
    //}
}