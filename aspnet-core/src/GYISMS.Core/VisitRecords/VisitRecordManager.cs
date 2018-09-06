

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.VisitRecords;


namespace GYISMS.VisitRecords
{
    /// <summary>
    /// VisitRecord领域层的业务管理
    ///</summary>
    public class VisitRecordManager :GYISMSDomainServiceBase, IVisitRecordManager
    {
    private readonly IRepository<VisitRecord,Guid> _visitrecordRepository;

        /// <summary>
            /// VisitRecord的构造方法
            ///</summary>
        public VisitRecordManager(IRepository<VisitRecord, Guid>
visitrecordRepository)
            {
            _visitrecordRepository =  visitrecordRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitVisitRecord()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
