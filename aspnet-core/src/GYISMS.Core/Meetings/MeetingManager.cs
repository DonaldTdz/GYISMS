

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Meetings;


namespace GYISMS.Meetings
{
    /// <summary>
    /// Meeting领域层的业务管理
    ///</summary>
    public class MeetingManager :GYISMSDomainServiceBase, IMeetingManager
    {
    private readonly IRepository<Meeting,Guid> _meetingRepository;

        /// <summary>
            /// Meeting的构造方法
            ///</summary>
        public MeetingManager(IRepository<Meeting, Guid>
meetingRepository)
            {
            _meetingRepository =  meetingRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitMeeting()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
