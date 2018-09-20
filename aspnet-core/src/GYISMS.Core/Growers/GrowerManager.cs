

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Growers;


namespace GYISMS.Growers
{
    /// <summary>
    /// Grower领域层的业务管理
    ///</summary>
    public class GrowerManager :GYISMSDomainServiceBase, IGrowerManager
    {
    private readonly IRepository<Grower,int> _growerRepository;

        /// <summary>
            /// Grower的构造方法
            ///</summary>
        public GrowerManager(IRepository<Grower, int>
growerRepository)
            {
            _growerRepository =  growerRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitGrower()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
