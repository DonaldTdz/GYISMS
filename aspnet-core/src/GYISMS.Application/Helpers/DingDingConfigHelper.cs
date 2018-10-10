using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Helpers
{
    public class DingDingConfigHelper
    {
        public static int VisitTaskAgentID = 190023627;
        public static int ChartAgentID = 193427841;
        public static int MeetingAgentID = 189293029;

        /// <summary>
        /// 获取任务拜访 AssessToken
        /// </summary>
        public static string GetVisitTaskAccessToken()
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey = "ding7xespi5yumrzraaq";
            request.Appsecret = "idKPu4wVaZjBKo6oUvxcwSQB7tExjEbPaBpVpCEOGlcZPsH4BDx-sKilG726-nC3";
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);
            return response.AccessToken;
        }

    }
}
