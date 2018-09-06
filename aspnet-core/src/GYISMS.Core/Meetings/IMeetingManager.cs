

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Meetings;


namespace GYISMS.Meetings
{
    public interface IMeetingManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitMeeting();



		//// custom codes
 
        //// custom codes end

    }
}
