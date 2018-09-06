

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.TaskExamines;


namespace GYISMS.TaskExamines
{
    /// <summary>
    /// TaskExamine领域层的业务管理
    ///</summary>
    public class TaskExamineManager :GYISMSDomainServiceBase, ITaskExamineManager
    {
    private readonly IRepository<TaskExamine,int> _taskexamineRepository;

        /// <summary>
            /// TaskExamine的构造方法
            ///</summary>
        public TaskExamineManager(IRepository<TaskExamine, int>
taskexamineRepository)
            {
            _taskexamineRepository =  taskexamineRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitTaskExamine()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
