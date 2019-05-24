using GYISMS.GYEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Charts.Dtos
{
    public class DistrictDto
    {
        public AreaCodeEnum? AreaType { get; set; }

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
        public int? Id { get; set; }
        public string Name { get; set; }

        public string District { get; set; }

        public int? Num { get; set; }

        public int? Status { get; set; }
        public AreaCodeEnum? AreaCode { get; set; }
        public string TimeGroup { get; set; }
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
                        AreaCode = item.AreaType,
                        District = item.District,
                        Name = "完成",
                        Num = item.CompleteNum,
                        Status = 2,
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        AreaCode = item.AreaType,
                        District = item.District,
                        Name = "待完成",
                        Num = item.VisitNum - item.CompleteNum - item.OverdueNum,
                        Status = 3,
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        AreaCode = item.AreaType,
                        District = item.District,
                        Name = "逾期",
                        Num = item.OverdueNum,
                        Status = 0,
                    });
                }
                return items;
            }
        }
    }

    #region 公共chart数据

    public class ChartDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

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

    public class ChartItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string GroupName { get; set; }

        public int? Num { get; set; }

        public int? Status { get; set; }
    }

    public class ChartDataDto
    {
        public ChartDataDto()
        {
            Datas = new List<ChartDto>();

        }

        public List<ChartDto> Datas { get; set; }

        public List<ChartItemDto> Items
        {
            get
            {
                var items = new List<ChartItemDto>();
                foreach (var item in Datas)
                {
                    items.Add(new ChartItemDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        GroupName = "完成",
                        Num = item.CompleteNum,
                        Status = 2,
                    });
                    items.Add(new ChartItemDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        GroupName = "待完成",
                        Num = item.VisitNum - item.CompleteNum - item.OverdueNum,
                        Status = 3,
                    });
                    items.Add(new ChartItemDto()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        GroupName = "逾期",
                        Num = item.OverdueNum,
                        Status = 0,
                    });
                }
                return items;
            }
        }
    }

    #endregion

    public class SheduleByTaskDto
    {
        public int Id { get; set; }

        //public TaskTypeEnum? TaskType { get; set; }
        public string TaskName { get; set; }

        public int? VisitNum { get; set; }

        public int? CompleteNum { get; set; }

        public int? ExpiredNum { get; set; }

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

    public class ChartByTaskDto
    {
        public ChartByTaskDto()
        {
            Tasks = new List<SheduleByTaskDto>();
            AreaItem = new List<ItemDetail>();
        }
        public List<ItemDetail> AreaItem { get; set; }
        public List<SheduleByTaskDto> Tasks { get; set; }
        public List<DistrictChartItemDto> Items
        {
            get
            {
                var items = new List<DistrictChartItemDto>();
                foreach (var item in Tasks)
                {
                    items.Add(new DistrictChartItemDto()
                    {
                        Id = item.Id,
                        District = item.TaskName,
                        Name = "完成",
                        Num = item.CompleteNum,
                        Status = 2,
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        Id = item.Id,
                        District = item.TaskName,
                        Name = "待完成",
                        Num = item.VisitNum - item.CompleteNum - item.ExpiredNum,
                        Status = 3,
                    });
                    items.Add(new DistrictChartItemDto()
                    {
                        Id = item.Id,
                        District = item.TaskName,
                        Name = "逾期",
                        Num = item.ExpiredNum,
                        Status = 0,
                    });
                }
                return items;
            }
        }
    }

    public class SheduleDetailDto
    {
        public Guid Id { get; set; }
        public AreaCodeEnum? AreaCode { get; set; }

        public TaskTypeEnum? TaskType { get; set; }

        public string TaskName { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime? EndTime { get; set; }

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
        public int? ExpiredNum { get; set; }

        public string AreaName
        {
            get
            {
                return AreaCode.ToString();
            }
        }

        public string TaskTypeName
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
        public string BeginTimeStr
        {
            get
            {
                return BeginTime.HasValue ? BeginTime.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }
        public string EndTimeStr
        {
            get
            {
                return EndTime.HasValue ? EndTime.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
        }
    }



    public class DistrictStatisDto
    {

        public string GroupName
        {
            get
            {
                if (Status.HasValue)
                {
                    return Status == 0 ? "逾期" : (Status == 2 ? "完成" : "待完成");
                }
                else
                {
                    return "";
                }

            }
        }
        public int? Status { get; set; }
        public AreaCodeEnum? AreaCode { get; set; }
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
        public int? ExpiredNum { get; set; }

        public string AreaName
        {
            get
            {
                if (AreaCode.HasValue)
                {
                    return AreaCode.ToString();
                }
                return string.Empty;
            }
        }

        public int? Num
        {
            get
            {
                if (Status.HasValue)
                {
                    if (Status == 0)
                    {
                        return ExpiredNum;
                    }
                    else if (Status == 3)
                    {
                        return VisitNum - CompleteNum - ExpiredNum;
                    }
                    else
                    {
                        return CompleteNum;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    //任务统计区县/部门完成率
    public class ItemDetail
    {
        public AreaCodeEnum? AreaCode { get; set; }
        public string Type { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public int? VisitNum { get; set; }
        public int? CompleteNum { get; set; }
        public string AreaName
        {
            get
            {
                return AreaCode.ToString();
            }
        }
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

    public class TaskChartDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public TaskTypeEnum Type { get; set; }
        public string Name { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public string Department { get; set; }

        public int? VisitNum { get; set; }
        public int? CompleteNum { get; set; }
    }

    #region 状态图表DTO
    public class SheduleByStatusDto
    {
        public int Id { get; set; }
        public string GroupName
        {
            get
            {
                if (Status.HasValue)
                {
                    return Status == 0 ? "逾期" : (Status == 2 ? "完成" : "待完成");
                }
                else
                {
                    return "";
                }

            }
        }
        public int? Status { get; set; }
        public string TaskName { get; set; }

        public int? VisitNum { get; set; }

        /// <summary>
        /// CompleteNum
        /// </summary>
        public int? CompleteNum { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? ExpiredNum { get; set; }
        public int? Num
        {
            get
            {
                if (Status.HasValue)
                {
                    if (Status == 0)
                    {
                        return ExpiredNum;
                    }
                    else if (Status == 3)
                    {
                        return VisitNum - CompleteNum - ExpiredNum;
                    }
                    else
                    {
                        return CompleteNum;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class ItemDetailByStatus
    {
        public AreaCodeEnum? AreaCode { get; set; }
        public string GroupName
        {
            get
            {
                if (Status.HasValue)
                {
                    return Status == 0 ? "逾期" : (Status == 2 ? "完成" : "待完成");
                }
                else
                {
                    return "";
                }

            }
        }
        public string Type { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public int? VisitNum { get; set; }

        /// <summary>
        /// CompleteNum
        /// </summary>
        public int? CompleteNum { get; set; }

        /// <summary>
        /// 逾期数
        /// </summary>
        public int? ExpiredNum { get; set; }
        public int? Status { get; set; }

        public int? Num
        {
            get
            {
                if (Status.HasValue)
                {
                    if (Status == 0)
                    {
                        return ExpiredNum;
                    }
                    else if (Status == 3)
                    {
                        return VisitNum - CompleteNum - ExpiredNum;
                    }
                    else
                    {
                        return CompleteNum;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        public string AreaName
        {
            get
            {
                return AreaCode.ToString();
            }
        }
    }
    public class DistrictChartByStatusDto
    {
        public DistrictChartByStatusDto()
        {
            TasksByStatus = new List<SheduleByStatusDto>();
            AreaItemByStatus = new List<ItemDetailByStatus>();
        }
        public List<ItemDetailByStatus> AreaItemByStatus { get; set; }
        public List<SheduleByStatusDto> TasksByStatus { get; set; }
    }

    public class TaskChartByStatusDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public TaskTypeEnum Type { get; set; }
        public string Name { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public string Department { get; set; }

        public int? VisitNum { get; set; }
        public int? CompleteNum { get; set; }
    }
    public class ChartByStatusDto
    {
        public ChartByStatusDto()
        {
            Tasks = new List<SheduleByStatusDto>();
            AreaItem = new List<ItemDetailByStatus>();
        }
        public List<ItemDetailByStatus> AreaItem { get; set; }
        public List<SheduleByStatusDto> Tasks { get; set; }
    }

    #endregion
}
