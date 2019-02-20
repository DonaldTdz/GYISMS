

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using GYISMS;
using GYISMS.Employees;
using GYISMS.GYEnums;
using GYISMS.Organizations;
using GYISMS.SystemDatas;
using Microsoft.EntityFrameworkCore;

namespace GYISMS.Employees
{
    /// <summary>
    /// Employee领域层的业务管理
    ///</summary>
    public class EmployeeManager : GYISMSDomainServiceBase, IEmployeeManager
    {
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Organization, long> _organizationRepository;
        private readonly IRepository<SystemData> _systemDataRepository;

        /// <summary>
        /// Employee的构造方法
        ///</summary>
        public EmployeeManager(IRepository<Employee, string> employeeRepository,
            IRepository<Organization, long> organizationRepository,
            IRepository<SystemData> systemDataRepository)
        {
            _employeeRepository = employeeRepository;
            _organizationRepository = organizationRepository;
            _systemDataRepository = systemDataRepository;
        }

        /// <summary>
        ///     初始化
        ///</summary>
        public void InitEmployee()
        {
           
        }

        //TODO:编写领域业务代码
        #region 获取用户区县

        private void GetAreaDeptList(long deptId, List<long> deptIdList)
        {
            var list = _organizationRepository.GetAll().Where(o => o.ParentId == deptId).Select(o => o.Id).ToList();
            deptIdList.AddRange(list);
            foreach (var id in list)
            {
                GetAreaDeptList(id, deptIdList);
            }
        }

        /// <summary>
        /// 部门是否存在该区县下面
        /// </summary>
        private bool ExistsArea(string userDepts, string areaCode)
        {
            var childrenDeptIdList = new List<long>();
            var chdStrDeptIdList = new List<string>();
            var zhqDeptIds = _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == areaCode).Select(s => s.Desc).First();
            if (string.IsNullOrEmpty(zhqDeptIds))
            {
                return false;
            }
            var deptIdArray = zhqDeptIds.Split(',');
            foreach (var deptid in deptIdArray)
            {
                childrenDeptIdList.Add(long.Parse(deptid));
                GetAreaDeptList(long.Parse(deptid), childrenDeptIdList);
            }

            chdStrDeptIdList = childrenDeptIdList.Select(c => "[" + c.ToString() + "]").ToList();
            var exist = chdStrDeptIdList.Any(c => userDepts.Contains(c));
            return exist;
        }

        public async Task<AreaCodeEnum> GetAreaCodeByUserIdAsync(string userId)
        {
            var employee = await _employeeRepository.GetAsync(userId);
            if (employee.AreaCode.HasValue && employee.AreaCode != AreaCodeEnum.None)//用户特定设置优先
            {
                return employee.AreaCode.Value;
            }
            if (ExistsArea(employee.Department, GYCode.昭化区))
            {
                return AreaCodeEnum.昭化区;
            }
            if (ExistsArea(employee.Department, GYCode.剑阁县))
            {
                return AreaCodeEnum.剑阁县;
            }
            if (ExistsArea(employee.Department, GYCode.旺苍县))
            {
                return AreaCodeEnum.旺苍县;
            }
            return AreaCodeEnum.None;
        }

        public async Task<AreaCodeEnum> GetDeptAreaCodeByUserIdAsync(string userId)
        {
            var employee = await _employeeRepository.GetAsync(userId);
            if (ExistsArea(employee.Department, GYCode.昭化区))
            {
                return AreaCodeEnum.昭化区;
            }
            if (ExistsArea(employee.Department, GYCode.剑阁县))
            {
                return AreaCodeEnum.剑阁县;
            }
            if (ExistsArea(employee.Department, GYCode.旺苍县))
            {
                return AreaCodeEnum.旺苍县;
            }
            return AreaCodeEnum.None;
        }

        #endregion

        #region 获取区县部门Id数组

        public async Task<string[]> GetAreaDeptIdArrayAsync(AreaCodeEnum? areaCode)
        {
            var areakey = string.Empty;
            switch (areaCode)
            {
                case AreaCodeEnum.昭化区:
                    {
                        areakey = GYCode.昭化区;
                    }
                    break;
                case AreaCodeEnum.剑阁县:
                    {
                        areakey = GYCode.剑阁县;
                    }
                    break;
                case AreaCodeEnum.旺苍县:
                    {
                        areakey = GYCode.旺苍县;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(areakey))
            {
                return new string[0];
            }
            //多个部门逗号分隔
            var deptIds = await _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == areakey).Select(s => s.Desc).FirstAsync();
            if (string.IsNullOrEmpty(deptIds))
            {
                return new string[0];
            }
            var deptList = new List<long>();
            var deptIdArray = deptIds.Split(',');
            foreach (var deptid in deptIdArray)
            {
                deptList.Add(long.Parse(deptid));
                GetAreaDeptList(long.Parse(deptid), deptList);
            }
         
            return deptList.Select(c => "[" + c.ToString() + "]").ToArray();
        }

        #endregion

        #region 获取部门及下面子部门Id

        public async Task<string[]> GetDeptIdArrayAsync(long deptId)
        {
            return await Task.Run(() =>
            {
                var deptList = new List<long>();
                deptList.Add(deptId);
                GetAreaDeptList(deptId, deptList);
                return deptList.Select(c => "[" + c.ToString() + "]").ToArray();
            });     
        }

        #endregion
    }
}
