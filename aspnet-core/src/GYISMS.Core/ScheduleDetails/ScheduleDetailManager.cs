

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.ScheduleDetails;


namespace GYISMS.ScheduleDetails
{
    /// <summary>
    /// ScheduleDetail领域层的业务管理
    ///</summary>
    public class ScheduleDetailManager :GYISMSDomainServiceBase, IScheduleDetailManager
    {
    private readonly IRepository<ScheduleDetail,Guid> _scheduledetailRepository;

        /// <summary>
            /// ScheduleDetail的构造方法
            ///</summary>
        public ScheduleDetailManager(IRepository<ScheduleDetail, Guid>
scheduledetailRepository)
            {
            _scheduledetailRepository =  scheduledetailRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitScheduleDetail()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
