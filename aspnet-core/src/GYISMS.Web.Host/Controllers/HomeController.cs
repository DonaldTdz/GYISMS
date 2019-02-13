using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using GYISMS.Controllers;
using Abp.Quartz;
using Abp.Threading;
using GYISMS.VisitTaskJobs;
using Quartz;

namespace GYISMS.Web.Host.Controllers
{
    public class HomeController : GYISMSControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IQuartzScheduleJobManager _jobManager;

        public HomeController(INotificationPublisher notificationPublisher, IQuartzScheduleJobManager jobManager)
        {
            _notificationPublisher = notificationPublisher;
            _jobManager = jobManager;
        }

        public IActionResult Index()
        {
            //QuartzScheduleJobs();
            //return Redirect("/swagger");
            return Redirect("/gyadmin/index.html");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }

        public async Task<ActionResult> ScheduleJob()
        {
            await _jobManager.ScheduleAsync<VisitTaskStatusJob>(job =>
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
            });

            await _jobManager.ScheduleAsync<SendTaskOverdueMsgJob>(job =>
            {
                job.WithIdentity("SendTaskOverdueMsgJob", "TaskGroup").WithDescription("A job to send msg.");
            },
            trigger =>
            {
                trigger//.StartAt(new DateTimeOffset(startTime))
                .StartNow()//一旦加入scheduler，立即生效
                .WithCronSchedule("0 0 9 * * ?")//每天上午9点执行
                .Build();
            });

            return Content("OK, scheduled!");
        }


        /// <summary>
        /// 调度jobs
        /// </summary>
        private void QuartzScheduleJobs()
        {
            //var startTime = DateTime.Today.AddHours(2);
            //if (startTime < DateTime.Now)
            //{
            //    startTime.AddDays(1);
            //}
            AsyncHelper.RunSync(() => _jobManager.ScheduleAsync<VisitTaskStatusJob>(job =>
            {
                job.WithIdentity("VisitTaskStatusJob", "TaskGroup").WithDescription("A job to update task status.");
            },
            trigger =>
            {
                trigger//.StartAt(new DateTimeOffset(startTime))
                .StartNow()//一旦加入scheduler，立即生效
                .WithCronSchedule("0 35 14 * * ?")//每天凌晨2点执行
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
        }
    }
}
