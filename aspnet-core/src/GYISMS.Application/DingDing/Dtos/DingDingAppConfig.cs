using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.DingDing.Dtos
{
    public class DingDingAppConfig
    {
        public string CorpId { get; set; }

        public string Appkey { get; set; }

        public string Appsecret { get; set; }

        public int AgentID { get; set; }
    }

    public class DingDingConfigCode
    {
        public static string CorpId = "CorpId";

        public static string Appkey = "Appkey";

        public static string Appsecret = "Appsecret";

        public static string AgentID = "AgentID";
    }

    public enum DingDingAppEnum
    {
        任务拜访 = 7,
        智能报表 = 8,
        会议申请 = 9,
        资料库 = 10
    }
}
