

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Schedules;


namespace GYISMS.Schedules
{
    public interface IScheduleManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitSchedule();



		//// custom codes
 
        //// custom codes end

    }
}
