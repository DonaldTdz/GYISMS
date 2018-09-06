
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.Meetings.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="MeetingAppPermissions" /> for all permission names. Meeting
    ///</summary>
    public class MeetingAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Meeting 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var meeting = administration.CreateChildPermission(MeetingAppPermissions.Meeting , L("Meetings"));
meeting.CreateChildPermission(MeetingAppPermissions.Meeting_Create, L("Create"));
meeting.CreateChildPermission(MeetingAppPermissions.Meeting_Edit, L("Edit"));
meeting.CreateChildPermission(MeetingAppPermissions.Meeting_Delete, L("Delete"));
meeting.CreateChildPermission(MeetingAppPermissions.Meeting_BatchDelete , L("BatchDelete"));
meeting.CreateChildPermission(MeetingAppPermissions.Meeting_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }