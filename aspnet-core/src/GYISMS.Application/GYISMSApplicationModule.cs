using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Quartz;
using Abp.Quartz.Configuration;
using Abp.Reflection.Extensions;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using GYISMS.Authorization;
using GYISMS.VisitTaskJobs;
using Quartz;

namespace GYISMS
{
    [DependsOn(
        typeof(GYISMSCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpQuartzModule))]
    public class GYISMSApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<GYISMSAuthorizationProvider>();
            //Quartz Job Config 2018-10-15
            Configuration.Modules.AbpQuartz().Scheduler.JobFactory = new AbpQuartzJobFactory(IocManager);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(GYISMSApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder.For<IDingDingAppService>("tasksystem/task").ForMethod("CreateTask").DontCreateAction().Build();
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IJobListener, AbpQuartzJobListener>();

            Configuration.Modules.AbpQuartz().Scheduler.ListenerManager.AddJobListener(IocManager.Resolve<IJobListener>());

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().Add(IocManager.Resolve<IQuartzScheduleJobManager>());
                ConfigureQuartzScheduleJobs();
            }
        }


        /// <summary>
        /// 调度jobs
        /// </summary>
        private void ConfigureQuartzScheduleJobs()
        {
            var jobManager = IocManager.Resolve<IQuartzScheduleJobManager>();
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
                .WithCronSchedule("0 0 2 * * ?")//每天凌晨2点执行
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
