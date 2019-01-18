using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.DingDing.Dtos;
using GYISMS.SystemDatas;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GYISMS.GYEnums;

namespace GYISMS.DingDing
{
    [AbpAllowAnonymous]
    [Audited]
    public class DingDingAppService : GYISMSAppServiceBase, IDingDingAppService
    {
        private readonly IRepository<SystemData> _systemDataRepository;

        public DingDingAppService(IRepository<SystemData> systemDataRepository)
        {
            _systemDataRepository = systemDataRepository;
        }

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

        /// <summary>
        /// 获取钉钉AccessToken 根据App
        /// </summary>
        public string GetAccessTokenByApp(DingDingAppEnum app)
        {
            var config = GetDingDingConfigByApp(app);
            return GetAccessToken(config.Appkey, config.Appsecret);
        }

        /// <summary>
        /// 获取钉钉配置根据 应用App
        /// </summary>
        public DingDingAppConfig GetDingDingConfigByApp(DingDingAppEnum app)
        {
            DingDingAppConfig config = new DingDingAppConfig();
            var configList = new List<SystemData>();
            switch (app)
            {
                case DingDingAppEnum.任务拜访:
                    {
                        configList = _systemDataRepository.GetAll()
                                    .Where(s => s.ModelId == ConfigModel.钉钉配置)
                                    .Where(s => s.Type == ConfigType.钉钉配置 || s.Type == ConfigType.任务拜访)
                                    .ToList();
                    }
                    break;
                case DingDingAppEnum.智能报表:
                    {
                        configList = _systemDataRepository.GetAll()
                                   .Where(s => s.ModelId == ConfigModel.钉钉配置)
                                   .Where(s => s.Type == ConfigType.钉钉配置 || s.Type == ConfigType.智能报表)
                                   .ToList();
                    }
                    break;
                case DingDingAppEnum.会议申请:
                    {
                        configList = _systemDataRepository.GetAll()
                                   .Where(s => s.ModelId == ConfigModel.钉钉配置)
                                   .Where(s => s.Type == ConfigType.钉钉配置 || s.Type == ConfigType.会议申请)
                                   .ToList();
                    }
                    break;
                case DingDingAppEnum.资料库:
                    {
                        configList = _systemDataRepository.GetAll()
                                   .Where(s => s.ModelId == ConfigModel.钉钉配置)
                                   .Where(s => s.Type == ConfigType.钉钉配置 || s.Type == ConfigType.企业标准库)
                                   .ToList();
                    }
                    break;

            }
            foreach (var item in configList)
            {
                if (item.Code == DingDingConfigCode.CorpId)
                {
                    config.CorpId = item.Desc;
                }
                else if (item.Code == DingDingConfigCode.Appkey)
                {
                    config.Appkey = item.Desc;
                }
                else if (item.Code == DingDingConfigCode.Appsecret)
                {
                    config.Appsecret = item.Desc;
                }
                else if (item.Code == DingDingConfigCode.AgentID)
                {
                    int outAgenId = 0;
                    if (int.TryParse(item.Desc, out outAgenId))
                    {
                        config.AgentID = outAgenId;
                    }
                }
            }
            return config;
        }
    }
}
