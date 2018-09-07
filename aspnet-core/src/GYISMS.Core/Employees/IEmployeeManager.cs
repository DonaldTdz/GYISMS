

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Employees;


namespace GYISMS.Employees
{
    public interface IEmployeeManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitEmployee();



		//// custom codes
 
        //// custom codes end

    }
}
