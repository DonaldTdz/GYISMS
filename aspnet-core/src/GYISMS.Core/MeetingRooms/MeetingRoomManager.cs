

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.MeetingRooms;


namespace GYISMS.MeetingRooms
{
    /// <summary>
    /// MeetingRoom领域层的业务管理
    ///</summary>
    public class MeetingRoomManager :GYISMSDomainServiceBase, IMeetingRoomManager
    {
    private readonly IRepository<MeetingRoom,int> _meetingroomRepository;

        /// <summary>
            /// MeetingRoom的构造方法
            ///</summary>
        public MeetingRoomManager(IRepository<MeetingRoom, int>
meetingroomRepository)
            {
            _meetingroomRepository =  meetingroomRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitMeetingRoom()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
