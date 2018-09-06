
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.ScheduleTasks.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="ScheduleTaskAppPermissions" /> for all permission names. ScheduleTask
    ///</summary>
    public class ScheduleTaskAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了ScheduleTask 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var scheduletask = administration.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask , L("ScheduleTasks"));
scheduletask.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask_Create, L("Create"));
scheduletask.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask_Edit, L("Edit"));
scheduletask.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask_Delete, L("Delete"));
scheduletask.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask_BatchDelete , L("BatchDelete"));
scheduletask.CreateChildPermission(ScheduleTaskAppPermissions.ScheduleTask_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }