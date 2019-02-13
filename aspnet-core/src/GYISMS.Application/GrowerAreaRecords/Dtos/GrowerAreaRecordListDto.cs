

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.GrowerAreaRecords;

namespace GYISMS.GrowerAreaRecords.Dtos
{
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




    }
}