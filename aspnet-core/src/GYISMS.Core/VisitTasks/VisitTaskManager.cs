

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.VisitTasks;


namespace GYISMS.VisitTasks
{
    /// <summary>
    /// VisitTask领域层的业务管理
    ///</summary>
    public class VisitTaskManager :GYISMSDomainServiceBase, IVisitTaskManager
    {
    private readonly IRepository<VisitTask,int> _visittaskRepository;

        /// <summary>
            /// VisitTask的构造方法
            ///</summary>
        public VisitTaskManager(IRepository<VisitTask, int>
visittaskRepository)
            {
            _visittaskRepository =  visittaskRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitVisitTask()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
