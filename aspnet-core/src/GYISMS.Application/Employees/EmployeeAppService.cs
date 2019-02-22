
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

using GYISMS.Employees.Authorization;
using GYISMS.Employees.Dtos;
using GYISMS.Employees;
using GYISMS.Authorization;
using GYISMS.DingDing;
using Abp.Auditing;
using GYISMS.SystemDatas;
using GYISMS.GYEnums;
using GYISMS.SystemDatas.Dtos;
using GYISMS.DingDing.Dtos;
using GYISMS.Organizations;

namespace GYISMS.Employees
{
    /// <summary>
    /// Employee应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class EmployeeAppService : GYISMSAppServiceBase, IEmployeeAppService
    {
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IEmployeeManager _employeeManager;
        private readonly IDingDingAppService _dingDingAppService;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly IRepository<Organization, long> _organizationRepository;
        private DingDingAppConfig ddConfig;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public EmployeeAppService(IRepository<Employee, string> employeeRepository
            , IEmployeeManager employeeManager
            , IDingDingAppService dingDingAppService
            , IRepository<SystemData, int> systemdataRepository
            , IRepository<Organization, long> organizationRepository
            )
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _dingDingAppService = dingDingAppService;
            _systemdataRepository = systemdataRepository;
            _organizationRepository = organizationRepository;
        }


        /// <summary>
        /// 获取Employee的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeListDto>> GetPagedEmployeeAsync(GetEmployeesInput input)
        {

            var query = _employeeRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Mobile), u => u.Mobile.Contains(input.Mobile))
                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrEmpty(input.DepartId), u => u.Department.Contains(input.DepartId));
            // TODO:根据传入的参数添加过滤条件

            var employeeCount = await query.CountAsync();

            var employees = await query
                .OrderBy(v => v.Department)
                    .ThenBy(v => v.Id).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

            return new PagedResultDto<EmployeeListDto>(
                employeeCount,
                employeeListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取EmployeeListDto信息
        /// </summary>
        public async Task<EmployeeListDto> GetEmployeeByIdAsync(string id)
        {
            var entity = await _employeeRepository.GetAsync(id);
            var entityDto = entity.MapTo<EmployeeListDto>();
            var area = await _employeeManager.GetDeptAreaCodeByUserIdAsync(id);
            entityDto.DeptArea = area == AreaCodeEnum.None? "无" : area.ToString();
            entityDto.DeptAreaCode = area;
            //如果没有指定区县 就 采用部门区县
            if (!entity.AreaCode.HasValue || entity.AreaCode == AreaCodeEnum.None)
            {
                //var area = await _employeeManager.GetAreaCodeByUserIdAsync(id);
                //entityDto.Area = entityDto.DeptArea;
                //entityDto.AreaCode = area;
                entityDto.IsDeptArea = true;
            }
            else
            {
                entityDto.IsDeptArea = false;
            }
            return entityDto;
        }

        [Audited]
        /// <summary>
        /// 添加或者修改Employee的公共方法
        /// </summary>
        public async Task CreateOrUpdateEmployee(CreateOrUpdateEmployeeInput input)
        {

            if (input.Employee.Id != null)
            {
                await UpdateEmployeeAsync(input.Employee);
            }
            else
            {
                await CreateEmployeeAsync(input.Employee);
            }
        }


        /// <summary>
        /// 新增Employee
        /// </summary>
        [AbpAuthorize(EmployeeAppPermissions.Employee_Create)]
        protected virtual async Task<EmployeeEditDto> CreateEmployeeAsync(EmployeeEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Employee>(input);

            entity = await _employeeRepository.InsertAsync(entity);
            return entity.MapTo<EmployeeEditDto>();
        }

        /// <summary>
        /// 编辑Employee
        /// </summary>
        [AbpAuthorize(EmployeeAppPermissions.Employee_Edit)]
        protected virtual async Task UpdateEmployeeAsync(EmployeeEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _employeeRepository.GetAsync(input.Id);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _employeeRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Employee信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(EmployeeAppPermissions.Employee_Delete)]
        public async Task DeleteEmployee(EntityDto<string> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _employeeRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Employee的方法
        /// </summary>
        [AbpAuthorize(EmployeeAppPermissions.Employee_BatchDelete)]
        public async Task BatchDeleteEmployeesAsync(List<string> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _employeeRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 根据部门节点获取员工信息
        /// </summary>
        public async Task<PagedResultDto<EmployeeListDto>> GetEmployeeListByIdAsync(GetEmployeesInput input)
        {
            if (input.DepartId == "1" || input.DepartId == null)
            {
                var deptArr = await _employeeManager.GetAreaDeptIdArrayAsync(input.AreaCode);//获取该区县下的部门和子部门列表
                var query = _employeeRepository.GetAll()
                    .WhereIf(input.AreaCode.HasValue, e => e.AreaCode == input.AreaCode || deptArr.Contains(e.Department))
                    .WhereIf(!string.IsNullOrEmpty(input.Mobile), u => u.Mobile.Contains(input.Mobile))
                    .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name));

                var employeeCount = await query.CountAsync();
                var employees = await query
                        .OrderBy(v => v.Department)
                        .ThenBy(v => v.Id).AsNoTracking()
                        .PageBy(input)
                        .ToListAsync();
                var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

                return new PagedResultDto<EmployeeListDto>(
                        employeeCount,
                        employeeListDtos
                    );
            }
            else
            {
                var query = _employeeRepository.GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.Mobile), u => u.Mobile.Contains(input.Mobile))
                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                    .Where(v => v.Department.Contains(input.DepartId));
                var employeeCount = await query.CountAsync();

                var employees = await query
                        .OrderBy(v => v.Id).AsNoTracking()
                        .PageBy(input)
                        .ToListAsync();
                var employeeListDtos = employees.MapTo<List<EmployeeListDto>>();

                return new PagedResultDto<EmployeeListDto>(
                        employeeCount,
                        employeeListDtos
                    );
            }
        }

        /// <summary>
        /// 获取区县Children
        /// </summary>
        private List<EmployeeNzTreeNode> GetAreaEmoloyee(AreaCodeEnum? area)
        {
            var employeeList = (from o in _employeeRepository.GetAll().Where(v => v.AreaCode == area)
                                select new EmployeeNzTreeNode()
                                {
                                    key = o.Id,
                                    title = o.Position.Length != 0 ? o.Name + $"({o.Position})" : o.Name,
                                    children = null,
                                    IsLeaf = true
                                }).ToList();
            return employeeList;
        }

        private List<EmployeeNzTreeNode> GetDeptChildren(long orgId)
        {
            var orgList = _organizationRepository.GetAll().Where(o => o.ParentId == orgId).ToList();
            var childrenList = new List<EmployeeNzTreeNode>();
            List<EmployeeNzTreeNode> treeNodeList = orgList.Select(t => new EmployeeNzTreeNode()
            {
                key = t.Id.ToString(),
                title = t.DepartmentName,
                IsLeaf = false,
                IsDept = true,
                children = GetDeptChildren(t.Id)
            }).ToList();
            childrenList.AddRange(treeNodeList);
            var employeeList = (from o in _employeeRepository.GetAll()
                                    .Where(v => v.Department.Contains("[" + orgId + "]"))
                                select new EmployeeNzTreeNode()
                                {
                                    key = o.Id,
                                    title = o.Position.Length != 0 ? o.Name + $"({o.Position})" : o.Name,
                                    children = null,
                                    IsDept = false,
                                    IsLeaf = true
                                }).ToList();
            childrenList.AddRange(employeeList);
            return childrenList;
        }

        /// <summary>
        /// 获取区县下面的钉钉组织架构
        /// </summary>
        private List<EmployeeNzTreeNode> GetAreaOrganization(AreaCodeEnum area)
        {
            //获取配置code
            var orgCode = "";
            switch (area)
            {
                case AreaCodeEnum.昭化区:
                    {
                        orgCode = GYCode.昭化区;
                    }
                    break;
                case AreaCodeEnum.剑阁县:
                    {
                        orgCode = GYCode.剑阁县;
                    }
                    break;
                case AreaCodeEnum.旺苍县:
                    {
                        orgCode = GYCode.旺苍县;
                    }
                    break;
            }
            var orgIds = _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == orgCode).First().Desc;
            List<EmployeeNzTreeNode> areaList = new List<EmployeeNzTreeNode>();
            if (!string.IsNullOrEmpty(orgIds))
            {
                var orgIdArr = orgIds.Split(',');
                foreach (var orgid in orgIdArr)
                {
                    var longOrgId = long.Parse(orgid);
                    var org = _organizationRepository.Get(longOrgId);
                    areaList.Add(new EmployeeNzTreeNode()
                    {
                        key = org.Id.ToString(),
                        title = org.DepartmentName,
                        IsDept = true,
                        IsLeaf = false,
                        children = GetDeptChildren(longOrgId)
                    });
                }
            }
            
            //添加特定访问人员（通过设置区县指定）
            var specificUserList = GetAreaEmoloyee(area);
            areaList.AddRange(specificUserList);
            return areaList;
        }

        private async Task<List<EmployeeNzTreeNode>> GetDeptEmployeeTreesAsync()
        {
            List<EmployeeNzTreeNode> treeNodeList = new List<EmployeeNzTreeNode>();
            //var AreaInfo = await _systemdataRepository.GetAll().Where(v => v.Type == ConfigType.烟农村组 && v.ModelId == ConfigModel.烟叶服务).OrderBy(v => v.Seq).AsNoTracking()
            // .Select(v => new SystemDataListDto() { Code = v.Code, Desc = v.Desc }).ToListAsync();
            //foreach (var item in AreaInfo)
            //{
            //    EmployeeNzTreeNode treeNode = new EmployeeNzTreeNode()
            //    {
            //        key = item.Code,
            //        title = item.Desc,
            //        children = GetAreaEmoloyee(item.Code)
            //    };
            //    treeNodeList.Add(treeNode);
            //}
            var areaCode = await GetCurrentUserAreaCodeAsync();

            var key = AreaCodeEnum.昭化区;
            if (!areaCode.HasValue || areaCode == AreaCodeEnum.昭化区)
            {
                EmployeeNzTreeNode treeNode = new EmployeeNzTreeNode()
                {
                    key = key.GetHashCode().ToString(),
                    title = AreaCodeEnum.昭化区.ToString(),
                    //children = GetAreaEmoloyee(key)
                    children = GetAreaOrganization(key)
                };
                treeNodeList.Add(treeNode);
            }
            if (!areaCode.HasValue || areaCode == AreaCodeEnum.剑阁县)
            {
                key = AreaCodeEnum.剑阁县;
                EmployeeNzTreeNode treeNode2 = new EmployeeNzTreeNode()
                {
                    key = key.GetHashCode().ToString(),
                    title = AreaCodeEnum.剑阁县.ToString(),
                    //children = GetAreaEmoloyee(key)
                    children = GetAreaOrganization(key)
                };
                treeNodeList.Add(treeNode2);
            }

            if (!areaCode.HasValue || areaCode == AreaCodeEnum.旺苍县)
            {
                key = AreaCodeEnum.旺苍县;
                EmployeeNzTreeNode treeNode3 = new EmployeeNzTreeNode()
                {
                    key = key.GetHashCode().ToString(),
                    title = AreaCodeEnum.旺苍县.ToString(),
                    //children = GetAreaEmoloyee(key)
                    children = GetAreaOrganization(key)
                };
                treeNodeList.Add(treeNode3);
            }

            return treeNodeList;
        }

        /// <summary>
        /// 获取区县树
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeNzTreeNode>> GetTreesAsync()
        {
            var tree = await GetDeptEmployeeTreesAsync();
            if (tree.Count > 0)
            {
                tree[0].selected = true;
            }
            return tree;
        }

        [AbpAllowAnonymous]
        [Audited]

        public async Task<DingDingUserDto> GetDingDingUserByCodeAsync(string code, DingDingAppEnum appId)
        {
            ddConfig = _dingDingAppService.GetDingDingConfigByApp(appId);
            //测试环境注释
            var assessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
            var userId = _dingDingAppService.GetUserId(assessToken, code);
            //var userId = "16550049332052666774";
            // var userId = "1926112826844702";
            var query = await _employeeRepository.GetAsync(userId);
            var dduser = query.MapTo<DingDingUserDto>();
            var area = await _employeeManager.GetAreaCodeByUserIdAsync(dduser.Id);//钉钉用户区县权限
            dduser.Area = area.ToString();
            dduser.AreaCode = area;
            return dduser;
        }

        /// <summary>
        /// 更新员工区县信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EmployeeListDto> EditEmployeeAreaInfoAsync(EmployeeEditDto input)
        {
            var entity = await _employeeRepository.GetAsync(input.Id);
            if (input.IsDeptArea)//如果是使用部门区县 清除当前设置区县
            {
                entity.Area = null;
                entity.AreaCode = null;
                var result = await _employeeRepository.UpdateAsync(entity);
                return result.MapTo<EmployeeListDto>();
            }
            else
            {
                if (entity.AreaCode != input.AreaCode && entity.Area != input.Area)
                {
                    entity.Area = input.Area;
                    entity.AreaCode = input.AreaCode;
                    var result = await _employeeRepository.UpdateAsync(entity);
                    return result.MapTo<EmployeeListDto>();
                }
                else
                {
                    return entity.MapTo<EmployeeListDto>();
                }
            }
           
        }

        #region 烟农页面层级树 add by donald 2019-2-12

        public async Task<List<EmployeeNzTreeNode>> GetGrowerTreesAsync()
        {
            List<EmployeeNzTreeNode> treeNodeList = await GetDeptEmployeeTreesAsync();

            //添加全部区县
            List<EmployeeNzTreeNode> allNodeList = new List<EmployeeNzTreeNode>();
            allNodeList.Add(new EmployeeNzTreeNode()
            {
                title = "广元市",
                key = "",
                children = treeNodeList,
                selected = true
            });

            return allNodeList;
        }

        #endregion
    }
}


