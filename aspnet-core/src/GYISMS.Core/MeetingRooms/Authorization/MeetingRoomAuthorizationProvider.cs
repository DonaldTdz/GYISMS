
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.MeetingRooms.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="MeetingRoomAppPermissions" /> for all permission names. MeetingRoom
    ///</summary>
    public class MeetingRoomAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了MeetingRoom 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var meetingroom = administration.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom , L("MeetingRooms"));
meetingroom.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom_Create, L("Create"));
meetingroom.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom_Edit, L("Edit"));
meetingroom.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom_Delete, L("Delete"));
meetingroom.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom_BatchDelete , L("BatchDelete"));
meetingroom.CreateChildPermission(MeetingRoomAppPermissions.MeetingRoom_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }