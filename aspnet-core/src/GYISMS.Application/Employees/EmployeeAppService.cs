
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

        /// <summary>
        /// 构造函数 
        ///</summary>
        public EmployeeAppService(IRepository<Employee, string> employeeRepository
            , IEmployeeManager employeeManager
            , IDingDingAppService dingDingAppService
            )
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _dingDingAppService = dingDingAppService;
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
        public async Task<EmployeeListDto> GetEmployeeByIdAsync(EntityDto<string> input)
        {
            var entity = await _employeeRepository.GetAsync(input.Id);

            return entity.MapTo<EmployeeListDto>();
        }


        /// <summary>
        /// 添加或者修改Employee的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeListDto>> GetEmployeeListByIdAsync(GetEmployeesInput input)
        {
            if (input.DepartId == "1" || input.DepartId == null)
            {
                var query = _employeeRepository.GetAll()
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
        /// 按需获取子节点
        /// </summary>
        //public async Task<List<EmployeeNzTreeNode>> GetTreesAsync()
        //{
        //    var employeeList = await (from o in _employeeRepository.GetAll()
        //                                  select new EmployeeListDto()
        //                                  {
        //                                      Id = o.Id,
        //                                      Name = o.Name,
        //                                  }).ToListAsync();
        //    return GetTrees(0
        //        , employeeList);
        //}

        //private List<EmployeeNzTreeNode> GetTrees(long? id, List<EmployeeListDto> employeeList)
        //{
        //    List<EmployeeNzTreeNode> treeNodeList = employeeList.Where(o => o.Department.Contains("1")).Select(t => new EmployeeNzTreeNode()
        //    {
        //        key = t.Id,
        //        title = t.Name,
        //        children = GetTrees(t.Id, employeeList)
        //    }).ToList();
        //    return treeNodeList;
        //}
        [AbpAllowAnonymous]
        public async Task<DingDingUserDto> GetDingDingUserByCodeAsync(string code)
        {
            //测试环境注释
            //var assessToken = _dingDingAppService.GetAccessToken("ding7xespi5yumrzraaq", "idKPu4wVaZjBKo6oUvxcwSQB7tExjEbPaBpVpCEOGlcZPsH4BDx-sKilG726-nC3");
            //var userId = _dingDingAppService.GetUserId(assessToken, code);
            var userId = "165500493321719640";
            var query = await _employeeRepository.GetAsync(userId);
            return query.MapTo<DingDingUserDto>();
        }
    }
}


