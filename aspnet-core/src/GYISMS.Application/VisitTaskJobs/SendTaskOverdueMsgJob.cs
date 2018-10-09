using Abp.Dependency;
using Abp.Quartz;
using GYISMS.ScheduleDetails;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GYISMS.VisitTaskJobs
{
    public class SendTaskOverdueMsgJob : JobBase, ITransientDependency
    {
        private readonly IScheduleDetailAppService _scheduleDetailAppService;

        public SendTaskOverdueMsgJob(IScheduleDetailAppService scheduleDetailAppService)
        {
            _scheduleDetailAppService = scheduleDetailAppService;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            await _scheduleDetailAppService.SendTaskOverdueMsgAsync();
        }
    }
}
