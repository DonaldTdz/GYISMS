

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.ScheduleDetails;


namespace GYISMS.ScheduleDetails
{
    public interface IScheduleDetailManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitScheduleDetail();



		//// custom codes
 
        //// custom codes end

    }
}
