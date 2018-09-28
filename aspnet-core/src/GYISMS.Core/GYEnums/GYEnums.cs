using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.GYEnums
{
    public enum ConfigType
    {
        设备配置 = 1,
        会议物资 = 2,
        烟农单位 = 3,
        烟农村组 = 4,
        烟叶公共 = 5
    }

    public enum ConfigModel
    {
        会议管理 = 1,
        烟叶服务 = 2
    }

    public enum RoomType
    {
        固定会议室 = 1,
        临时会议室 = 2
    }

    public enum LayoutPattern
    {
        中心模式 = 1,
        矩阵莫事 = 2
    }

    public enum ScheduleType
    {
        每日 = 1,
        每周 = 2,
        每月 = 3
    }

    public enum ScheduleMasterStatusEnum
    {
        草稿 = 0,
        已发布 = 1
    }

    public enum ScheduleStatusEnum
    {
        已逾期 = 0,
        未开始 = 1,
        进行中 = 2,
        已完成 = 3
    }

    public enum TaskTypeEnum
    {
        技术服务 = 1,
        生产管理 = 2,
        政策宣传 = 3,
        临时任务 = 4
    }

    public enum NoticeWayEnum
    {
        发DING = 0,
        钉钉消息 = 1
    }
    public enum RemindingWayEnum
    {
        无提醒 = 0,
        发DING = 1,
        钉钉消息 =2
    }
    public enum RemindingTimeEnum
    {
        提前5分钟 = 0,
        提前10分钟 = 1,
        提前30分钟 = 2
    }
}
