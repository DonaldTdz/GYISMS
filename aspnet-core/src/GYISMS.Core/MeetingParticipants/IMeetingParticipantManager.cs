

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.MeetingParticipants;


namespace GYISMS.MeetingParticipants
{
    public interface IMeetingParticipantManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitMeetingParticipant();



		//// custom codes
 
        //// custom codes end

    }
}
