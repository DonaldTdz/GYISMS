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
        烟叶公共 = 5,
        钉钉配置 = 6,
        任务拜访 = 7,
        智能报表 = 8,
        会议申请 = 9,
        企业标准库 = 10,
        标准库角色 = 11
    }

    public enum ConfigModel
    {
        会议管理 = 1,
        烟叶服务 = 2,
        钉钉配置 = 3
    }

    public enum RoomType
    {
        固定会议室 = 1,
        临时会议室 = 2
    }

    public enum LayoutPattern
    {
        中心模式 = 1,
        矩阵模式 = 2
    }

    public enum ScheduleType
    {
        每月 = 1,
        每周 = 2,
        每日 = 3,
        自定义=4,
    }

    public enum ScheduleMasterStatusEnum
    {
        草稿 = 0,
        已发布 = 1
    }

    public enum ScheduleStatusEnum
    {
        None = -1,
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
        面积落实 = 5,
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
        钉钉消息 = 2
    }
    public enum RemindingTimeEnum
    {
        提前5分钟 = 0,
        提前10分钟 = 1,
        提前30分钟 = 2
    }
    public enum AreaCodeEnum
    {
        None = 0,
        昭化区 = 1,
        剑阁县 = 2,
        旺苍县 = 3,
        广元市 = 4
    }

    public enum FileTypeEnum
    {
        PDF = 1,
        Word = 2,
        Excel = 3,
        Other = 4
    }

    public enum ExamineOptionEnum
    {
        优差等级 = 1, //优/合格/差
        到位情况 = 2, //到位/不到位
        了解情况 = 3  //了解/不了解
    }

    public enum AreaStatusEnum
    {
        未落实 = 0,
        已落实 = 1
    }

    public static class GYCode
    {
        public static string SignRange = "SignRange";
        public static string MessageTitle = "MessageTitle";
        public static string MediaId = "MediaId";
        public static string ZHQPID = "ZHQPID";
        public static string JGXPID = "JGXPID";
        public static string WCXPID = "WCXPID";
        public static string LocationLimitCode = "LocationLimitCode";
        public static string DocMediaId = "DocMediaId"; 
        public static string DeptArry = "DeptArry";
        /// <summary>
        /// 昭化区 组织代码
        /// </summary>
        public static string 昭化区 = "ZHQ_ORG_CODE";
        /// <summary>
        /// 剑阁县 组织代码
        /// </summary>
        public static string 剑阁县 = "JGX_ORG_CODE";
        /// <summary>
        /// 旺苍县 组织代码
        /// </summary>
        public static string 旺苍县 = "WCX_ORG_CODE";

        public static string GaoDeAPIKey = "GaoDeAPIKey";
        public static string TaskApp = "TaskApp";
    }
}
