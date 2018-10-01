

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.ScheduleDetails;
using GYISMS.GYEnums;

namespace GYISMS.ScheduleDetails.Dtos
{
    public class ScheduleDetailListDto : EntityDto<Guid>
    {

        /// <summary>
        /// TaskId
        /// </summary>
        [Required(ErrorMessage = "TaskId不能为空")]
        public int TaskId { get; set; }


        /// <summary>
        /// ScheduleId
        /// </summary>
        [Required(ErrorMessage = "ScheduleId不能为空")]
        public Guid ScheduleId { get; set; }


        /// <summary>
        /// EmployeeId
        /// </summary>
        [Required(ErrorMessage = "EmployeeId不能为空")]
        public long EmployeeId { get; set; }


        /// <summary>
        /// GrowerId
        /// </summary>
        [Required(ErrorMessage = "GrowerId不能为空")]
        public int GrowerId { get; set; }


        /// <summary>
        /// VisitNum
        /// </summary>
        public int? VisitNum { get; set; }


        /// <summary>
        /// CompleteNum
        /// </summary>
        public int? CompleteNum { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }


        public ScheduleStatusEnum Status { get; set; }


        public Guid ScheduleTaskId { get; set; }

        public string EmployeeName { get; set; }

        public string GrowerName { get; set; }
    }
    public class HomeInfo
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 已完成数
        /// </summary>
        public int Completed { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public string CompletedRate { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int Expired { get; set; }

    }

    public class SheduleStatisticalDto
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// 已完成数
        /// </summary>
        public int? Completed { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? Expired { get; set; }
    }

    public class SheduleSumStatisDto
    {
        /// <summary>
        /// 计划明细
        /// </summary>
        public List<SheduleSumDto> sheduleSumDtos { get; set; }

        /// <summary>
        /// 计划总数
        /// </summary>
        public int TotalSum { get; set; }

        /// <summary>
        /// 完成总数
        /// </summary>
        public int CompleteSum { get; set; }

        /// <summary>
        /// 逾期总数
        /// </summary>
        public int ExpiredSum { get; set; }

    }

    public class SheduleDetailTaskListDto
    {

        public Guid Id { get; set; }
        /// <summary>
        /// AreaCode
        /// </summary>
        public AreaTypeEnum? AreaCode { get; set; }

        /// <summary>
        /// TaskName
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// TaskName
        /// </summary>
        public TaskTypeEnum? TaskType { get; set; }

        /// <summary>
        /// VisitNum
        /// </summary>
        public int? VisitNum { get; set; }

        /// <summary>
        /// CompleteNum
        /// </summary>
        public int? CompleteNum { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? Expired { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ScheduleStatusEnum Status { get; set; }

        /// <summary>
        /// 烟技员
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 烟农
        /// </summary>
        public string GrowerName { get; set; }

        public string AreaName
        {
            get
            {
                return AreaCode.ToString();
            }
        }

        public string TypeName
        {
            get
            {
                return TaskType.ToString();
            }
        }

        public string StatusName
        {
            get
            {
                return Status.ToString();
            }
        }
    }


}