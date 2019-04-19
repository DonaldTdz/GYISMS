

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.GrowerAreaRecords;
using Abp.AutoMapper;
using System.Collections.Generic;
using GYISMS.Employees.Dtos;
using Abp.Domain.Entities;
using GYISMS.GrowerLocationLogs;

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

        public string ImgTop
        {
            get
            {
                if (ImgPaths.Length > 0)
                {
                    return ImgPaths[0];
                }
                return string.Empty;
            }
        }

        public string[] ImgPaths
        {
            get
            {
                if (!string.IsNullOrEmpty(ImgPath))
                {
                    return ImgPath.Split(',');
                }
                return new string[0];
            }
        }

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
        public string DepartmentId { get; set; }
        public string AreaName { get; set; }
        public decimal Actual { get; set; }
        public decimal Expected { get; set; }
    }

    /// <summary>
    /// 通用图表dto
    /// </summary>
    public class CommChartDto
    {
        public string AreaName { get; set; }
        public decimal Area { get; set; }
        public string GroupName { get; set; }
        //public decimal Actual { get; set; }
        //public decimal Expected { get; set; }
        //public AreaDetailDto Detail{get;set;}
    }
    public class CommDetail
    {
        public CommDetail()
        {
            List = new List<CommChartDto>();
            Detail = new List<AreaDetailDto>();
        }
        public List<CommChartDto> List { get; set; }
        public List<AreaDetailDto> Detail { get; set; }
        //0 部门 1烟技员
        public int Type { get; set; }
    }

    /// <summary>
    /// 返回部门列表和其他dto
    /// </summary>
    public class DepartMentAndOther
    {
        public DepartMentAndOther()
        {
            Children = new List<EmployeeNzTreeNode>();
        }
        public string OtherIds { get; set; }
        public List<EmployeeNzTreeNode> Children { get; set; }
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
    [AutoMapFrom(typeof(GrowerAreaRecord))]
    public class AppGrowerAreaRecord: EntityDto<Guid>
    {
        public int GrowerId { get; set; }

        /// <summary>
        /// 计划明细Id 外键
        /// </summary>
        public Guid ScheduleDetailId { get; set; }

        /// <summary>
        /// 拍照图片
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 采集人快照（烟技员名称）
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 采集人快照（烟技员Id）
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 采集面积
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    [AutoMapFrom(typeof(GrowerLocationLog))]

    public class APPGrowerLocationLog : Entity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        //public Guid Id { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 烟农
        /// </summary>
        public int GrowerId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

    }
}