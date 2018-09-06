
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.VisitTasks.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="VisitTaskAppPermissions" /> for all permission names. VisitTask
    ///</summary>
    public class VisitTaskAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了VisitTask 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var visittask = administration.CreateChildPermission(VisitTaskAppPermissions.VisitTask , L("VisitTasks"));
visittask.CreateChildPermission(VisitTaskAppPermissions.VisitTask_Create, L("Create"));
visittask.CreateChildPermission(VisitTaskAppPermissions.VisitTask_Edit, L("Edit"));
visittask.CreateChildPermission(VisitTaskAppPermissions.VisitTask_Delete, L("Delete"));
visittask.CreateChildPermission(VisitTaskAppPermissions.VisitTask_BatchDelete , L("BatchDelete"));
visittask.CreateChildPermission(VisitTaskAppPermissions.VisitTask_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }