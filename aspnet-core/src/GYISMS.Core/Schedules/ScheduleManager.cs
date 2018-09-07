

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Schedules;


namespace GYISMS.Schedules
{
    /// <summary>
    /// Schedule领域层的业务管理
    ///</summary>
    public class ScheduleManager :GYISMSDomainServiceBase, IScheduleManager
    {
    private readonly IRepository<Schedule,Guid> _scheduleRepository;

        /// <summary>
            /// Schedule的构造方法
            ///</summary>
        public ScheduleManager(IRepository<Schedule, Guid>
scheduleRepository)
            {
            _scheduleRepository =  scheduleRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitSchedule()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
