

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.ScheduleTasks;


namespace GYISMS.ScheduleTasks
{
    public interface IScheduleTaskManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitScheduleTask();



		//// custom codes
 
        //// custom codes end

    }
}
