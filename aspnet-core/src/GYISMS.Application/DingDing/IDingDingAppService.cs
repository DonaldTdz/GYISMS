﻿using Abp.Application.Services;
using DingTalk.Api.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.DingDing
{
    public interface IDingDingAppService : IApplicationService
    {
        string GetAccessToken(string appkey, string appsecret);

        string GetUserId(string accessToken, string code);

        OapiUserGetResponse GetUserInfo(string accessToken, string userId);
    }
}