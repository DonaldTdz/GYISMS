
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.Employees.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="EmployeeAppPermissions" /> for all permission names. Employee
    ///</summary>
    public class EmployeeAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Employee 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var employee = administration.CreateChildPermission(EmployeeAppPermissions.Employee , L("Employees"));
employee.CreateChildPermission(EmployeeAppPermissions.Employee_Create, L("Create"));
employee.CreateChildPermission(EmployeeAppPermissions.Employee_Edit, L("Edit"));
employee.CreateChildPermission(EmployeeAppPermissions.Employee_Delete, L("Delete"));
employee.CreateChildPermission(EmployeeAppPermissions.Employee_BatchDelete , L("BatchDelete"));
employee.CreateChildPermission(EmployeeAppPermissions.Employee_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }