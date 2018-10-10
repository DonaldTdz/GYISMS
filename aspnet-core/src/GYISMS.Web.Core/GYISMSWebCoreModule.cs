using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using GYISMS.Authentication.JwtBearer;
using GYISMS.Configuration;
using GYISMS.EntityFrameworkCore;
using Abp.Quartz;
using Abp.Threading;
using GYISMS.VisitTaskJobs;
using Quartz;

namespace GYISMS
{
    [DependsOn(
         typeof(GYISMSApplicationModule),
         typeof(GYISMSEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        ,typeof(AbpAspNetCoreSignalRModule)
     )]
    public class GYISMSWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public GYISMSWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                GYISMSConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(GYISMSApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();

            //ConfigureQuartzScheduleJobs();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GYISMSWebCoreModule).GetAssembly());
        }

        /// <summary>
        /// 调度jobs
        /// </summary>
        private void ConfigureQuartzScheduleJobs()
        {
            IocManager.Register<QuartzScheduleJobManager>();
            var jobManager = IocManager.Resolve<QuartzScheduleJobManager>();
            //var startTime = DateTime.Today.AddHours(2);
            //if (startTime < DateTime.Now)
            //{
            //    startTime.AddDays(1);
            //}
            AsyncHelper.RunSync(() => jobManager.ScheduleAsync<VisitTaskStatusJob>(job =>
            {
                job.WithIdentity("VisitTaskStatusJob", "TaskGroup").WithDescription("A job to update task status.");
            },
            trigger =>
            {
                trigger//.StartAt(new DateTimeOffset(startTime))
                .StartNow()//一旦加入scheduler，立即生效
                .WithCronSchedule("0 36 15 * * ?")//每天凌晨2点执行
                .Build();
                //.StartNow()//一旦加入scheduler，立即生效
                /*.WithSimpleSchedule(schedule =>//使用SimpleTrigger
                {
                    schedule.RepeatForever()  //一直执行，奔腾到老不停歇
                    .WithIntervalInHours(24)  // 每隔24小时执行一次
                    //.WithIntervalInSeconds(5) // 每隔5秒执行一次
                    .Build();
                });*/
            }));

            AsyncHelper.RunSync(() => jobManager.ScheduleAsync<SendTaskOverdueMsgJob>(job =>
            {
                job.WithIdentity("SendTaskOverdueMsgJob", "TaskGroup").WithDescription("A job to send msg.");
            },
            trigger =>
            {
                trigger//.StartAt(new DateTimeOffset(startTime))
                .StartNow()//一旦加入scheduler，立即生效
                .WithCronSchedule("0 0 9 * * ?")//每天上午9点执行
                .Build();
            }));
        }
    }
}
