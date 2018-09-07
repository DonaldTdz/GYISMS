

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.MeetingRooms;


namespace GYISMS.MeetingRooms
{
    public interface IMeetingRoomManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitMeetingRoom();



		//// custom codes
 
        //// custom codes end

    }
}
