

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.SystemDatas;


namespace GYISMS.SystemDatas
{
    /// <summary>
    /// SystemData领域层的业务管理
    ///</summary>
    public class SystemDataManager :GYISMSDomainServiceBase, ISystemDataManager
    {
    private readonly IRepository<SystemData,int> _systemdataRepository;

        /// <summary>
            /// SystemData的构造方法
            ///</summary>
        public SystemDataManager(IRepository<SystemData, int>
systemdataRepository)
            {
            _systemdataRepository =  systemdataRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitSystemData()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
