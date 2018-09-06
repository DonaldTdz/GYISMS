

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.MeetingParticipants;


namespace GYISMS.MeetingParticipants
{
    /// <summary>
    /// MeetingParticipant领域层的业务管理
    ///</summary>
    public class MeetingParticipantManager :GYISMSDomainServiceBase, IMeetingParticipantManager
    {
    private readonly IRepository<MeetingParticipant,Guid> _meetingparticipantRepository;

        /// <summary>
            /// MeetingParticipant的构造方法
            ///</summary>
        public MeetingParticipantManager(IRepository<MeetingParticipant, Guid>
meetingparticipantRepository)
            {
            _meetingparticipantRepository =  meetingparticipantRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitMeetingParticipant()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
