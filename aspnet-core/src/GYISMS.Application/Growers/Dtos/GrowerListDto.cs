

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Growers;
using Abp.AutoMapper;
using GYISMS.GYEnums;

namespace GYISMS.Growers.Dtos
{
    [AutoMapFrom(typeof(Grower))]
    public class GrowerListDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Year
        /// </summary>
        public int? Year { get; set; }


        /// <summary>
        /// UnitCode
        /// </summary>
        public string UnitCode { get; set; }


        /// <summary>
        /// UnitName
        /// </summary>
        public string UnitName { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


        /// <summary>
        /// CountyCode
        /// </summary>
        public AreaCodeEnum? AreaCode { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }


        /// <summary>
        /// ContractNo
        /// </summary>
        public string ContractNo { get; set; }


        /// <summary>
        /// VillageGroup
        /// </summary>
        public string VillageGroup { get; set; }


        /// <summary>
        /// Tel
        /// </summary>
        public string Tel { get; set; }


        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        public int? Type { get; set; }


        /// <summary>
        /// PlantingArea
        /// </summary>
        public decimal? PlantingArea { get; set; }


        /// <summary>
        /// Longitude
        /// </summary>
        public decimal? Longitude { get; set; }


        /// <summary>
        /// Latitude
        /// </summary>
        public decimal? Latitude { get; set; }


        /// <summary>
        /// ContractTime
        /// </summary>
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        public Guid? ScheduleDetailId { get; set; }
        public int? VisitNum { get; set; }

        public string CountyCodeName
        {
            get
            {
                return AreaCode.ToString();
            }
        }
        public bool Checked { get; set; }

        /// <summary>
        /// 采集次数
        /// </summary>
        public int CollectNum { get; set; }

        public int? LimitNum { get; set; }


        /// <summary>
        ///预计单产（单位：担/亩）2019-2-13
        /// </summary>
        public decimal? UnitVolume { get; set; }

        /// <summary>
        /// 落实面积 2019-2-13
        /// </summary>
        public decimal? ActualArea { get; set; }

        /// <summary>
        /// 落实面积状态（枚举：未落实 0， 已落实 1）2019-2-13
        /// </summary>
        public AreaStatusEnum? AreaStatus { get; set; }

        /// <summary>
        /// 落实面积时间 2019-2-13
        /// </summary>
        public DateTime? AreaTime { get; set; }

        /// <summary>
        /// 采用落实面积的计划明细Id 2019-2-15
        /// </summary>
        public Guid? AreaScheduleDetailId { get; set; }

        public string AreaStatusName
        {
            get
            {
                return AreaStatus.ToString();
            }
        }
    }
}