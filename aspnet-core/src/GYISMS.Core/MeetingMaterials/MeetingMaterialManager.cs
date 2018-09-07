

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.MeetingMaterials;


namespace GYISMS.MeetingMaterials
{
    /// <summary>
    /// MeetingMaterial领域层的业务管理
    ///</summary>
    public class MeetingMaterialManager :GYISMSDomainServiceBase, IMeetingMaterialManager
    {
    private readonly IRepository<MeetingMaterial,Guid> _meetingmaterialRepository;

        /// <summary>
            /// MeetingMaterial的构造方法
            ///</summary>
        public MeetingMaterialManager(IRepository<MeetingMaterial, Guid>
meetingmaterialRepository)
            {
            _meetingmaterialRepository =  meetingmaterialRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitMeetingMaterial()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
