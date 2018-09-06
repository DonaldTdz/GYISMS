

using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Organizations;


namespace GYISMS.Organizations
{
    /// <summary>
    /// Organization领域层的业务管理
    ///</summary>
    public class OrganizationManager :GYISMSDomainServiceBase, IOrganizationManager
    {
    private readonly IRepository<Organization,long> _organizationRepository;

        /// <summary>
            /// Organization的构造方法
            ///</summary>
        public OrganizationManager(IRepository<Organization, long>
organizationRepository)
            {
            _organizationRepository =  organizationRepository;
            }


            /// <summary>
                ///     初始化
                ///</summary>
            public void InitOrganization()
            {
            throw new NotImplementedException();
            }

            //TODO:编写领域业务代码



            //// custom codes
             
            //// custom codes end

            }
            }
