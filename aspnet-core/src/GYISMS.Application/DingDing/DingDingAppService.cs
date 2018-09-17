using Abp.Authorization;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.DingDing
{
    [AbpAllowAnonymous]
    public class DingDingAppService : GYISMSAppServiceBase, IDingDingAppService
    {
        /// <summary>
        /// 获取addess token
        /// </summary>
        public string GetAccessToken(string appkey, string appsecret)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey = appkey;
            request.Appsecret = appsecret;
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);
            return response.AccessToken;
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        public string GetUserId(string accessToken, string code)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getuserinfo");
            OapiUserGetuserinfoRequest request = new OapiUserGetuserinfoRequest();
            request.Code = code;
            request.SetHttpMethod("GET");
            OapiUserGetuserinfoResponse response = client.Execute(request, accessToken);
            return response.Userid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public OapiUserGetResponse GetUserInfo(string accessToken, string userId)
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
            OapiUserGetRequest request = new OapiUserGetRequest();
            request.Userid = userId;
            request.SetHttpMethod("GET");
            return client.Execute(request, accessToken);
        }
    }
}
