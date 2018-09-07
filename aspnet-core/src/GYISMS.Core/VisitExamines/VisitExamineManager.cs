

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.VisitExamines;


namespace GYISMS.VisitExamines
{
    /// <summary>
    /// VisitExamine领域层的业务管理
    ///</summary>
    public class VisitExamineManager :GYISMSDomainServiceBase, IVisitExamineManager
    {
    private readonly IRepository<VisitExamine,Guid> _visitexamineRepository;

        /// <summary>
            /// VisitExamine的构造方法
            ///</summary>
        public VisitExamineManager(IRepository<VisitExamine, Guid>
visitexamineRepository)
            {
            _visitexamineRepository =  visitexamineRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitVisitExamine()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
