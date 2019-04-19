

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.Employees.Dtos;
using GYISMS.Employees;
using GYISMS.DingDing.Dtos;
using GYISMS.Dtos;

namespace GYISMS.Employees
{
    /// <summary>
    /// Employee应用层服务的接口方法
    ///</summary>
    public interface IEmployeeAppService : IApplicationService
    {
        /// <summary>
    /// 获取Employee的分页列表信息
    ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeListDto>> GetPagedEmployeeAsync(GetEmployeesInput input);

		/// <summary>
		/// 通过指定id获取EmployeeListDto信息
		/// </summary>
		Task<EmployeeListDto> GetEmployeeByIdAsync(string id);

        /// <summary>
        /// 导出Employee为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetEmployeesToExcel();


        /// <summary>
        /// 添加或者修改Employee的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateEmployee(CreateOrUpdateEmployeeInput input);


        /// <summary>
        /// 删除Employee信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteEmployee(EntityDto<string> input);


        /// <summary>
        /// 批量删除Employee
        /// </summary>
        Task BatchDeleteEmployeesAsync(List<string> input);
        Task<PagedResultDto<EmployeeListDto>> GetEmployeeListByIdAsync(GetEmployeesInput input);

        Task<DingDingUserDto> GetDingDingUserByCodeAsync(string code, DingDingAppEnum appId);

        Task<EmployeeListDto> EditEmployeeAreaInfoAsync(EmployeeEditDto input);
        //List<EmployeeNzTreeNode> GetTrees();

        Task<List<EmployeeNzTreeNode>> GetTreesAsync();

        Task<List<EmployeeNzTreeNode>> GetGrowerTreesAsync();
        Task BatchUpdateDocRoleAsync(GetBatchDocRoleInput input);
        Task<APIResultDto> AppLoginAsnyc(AppLoginInfo input);
    }
}
