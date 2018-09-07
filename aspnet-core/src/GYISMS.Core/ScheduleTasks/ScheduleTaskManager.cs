

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.ScheduleTasks;


namespace GYISMS.ScheduleTasks
{
    /// <summary>
    /// ScheduleTask领域层的业务管理
    ///</summary>
    public class ScheduleTaskManager :GYISMSDomainServiceBase, IScheduleTaskManager
    {
    private readonly IRepository<ScheduleTask,Guid> _scheduletaskRepository;

        /// <summary>
            /// ScheduleTask的构造方法
            ///</summary>
        public ScheduleTaskManager(IRepository<ScheduleTask, Guid>
scheduletaskRepository)
            {
            _scheduletaskRepository =  scheduletaskRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitScheduleTask()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
