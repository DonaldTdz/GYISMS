
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.MeetingParticipants.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="MeetingParticipantAppPermissions" /> for all permission names. MeetingParticipant
    ///</summary>
    public class MeetingParticipantAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了MeetingParticipant 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var meetingparticipant = administration.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant , L("MeetingParticipants"));
meetingparticipant.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant_Create, L("Create"));
meetingparticipant.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant_Edit, L("Edit"));
meetingparticipant.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant_Delete, L("Delete"));
meetingparticipant.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant_BatchDelete , L("BatchDelete"));
meetingparticipant.CreateChildPermission(MeetingParticipantAppPermissions.MeetingParticipant_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }