

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Employees;
using GYISMS.GYEnums;

namespace GYISMS.Employees
{
    public interface IEmployeeManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitEmployee();


        /// <summary>
        /// 获取用户区县
        /// </summary>
        Task<AreaCodeEnum> GetAreaCodeByUserIdAsync(string userId);

        /// <summary>
        /// 获取部门区县
        /// </summary>
        Task<AreaCodeEnum> GetDeptAreaCodeByUserIdAsync(string userId);

        Task<string[]> GetAreaDeptIdArrayAsync(AreaCodeEnum? areaCode);

        /// <summary>
        /// 获取部门及下面子部门
        /// </summary>
        Task<string[]> GetDeptIdArrayAsync(long deptId);

    }
}
