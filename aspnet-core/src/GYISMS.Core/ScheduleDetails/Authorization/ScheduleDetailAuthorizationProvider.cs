
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.ScheduleDetails.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="ScheduleDetailAppPermissions" /> for all permission names. ScheduleDetail
    ///</summary>
    public class ScheduleDetailAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了ScheduleDetail 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var scheduledetail = administration.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail , L("ScheduleDetails"));
scheduledetail.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail_Create, L("Create"));
scheduledetail.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail_Edit, L("Edit"));
scheduledetail.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail_Delete, L("Delete"));
scheduledetail.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail_BatchDelete , L("BatchDelete"));
scheduledetail.CreateChildPermission(ScheduleDetailAppPermissions.ScheduleDetail_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }