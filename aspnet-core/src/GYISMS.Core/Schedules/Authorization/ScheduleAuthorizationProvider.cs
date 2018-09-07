
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.Schedules.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="ScheduleAppPermissions" /> for all permission names. Schedule
    ///</summary>
    public class ScheduleAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Schedule 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var schedule = administration.CreateChildPermission(ScheduleAppPermissions.Schedule , L("Schedules"));
schedule.CreateChildPermission(ScheduleAppPermissions.Schedule_Create, L("Create"));
schedule.CreateChildPermission(ScheduleAppPermissions.Schedule_Edit, L("Edit"));
schedule.CreateChildPermission(ScheduleAppPermissions.Schedule_Delete, L("Delete"));
schedule.CreateChildPermission(ScheduleAppPermissions.Schedule_BatchDelete , L("BatchDelete"));
schedule.CreateChildPermission(ScheduleAppPermissions.Schedule_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }