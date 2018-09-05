

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Organizations;


namespace GYISMS.Organizations
{
    public interface IOrganizationManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitOrganization();



		//// custom codes
 
        //// custom codes end

    }
}
