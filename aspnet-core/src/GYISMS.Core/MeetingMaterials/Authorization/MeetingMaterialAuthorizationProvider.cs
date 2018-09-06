
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.MeetingMaterials.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="MeetingMaterialAppPermissions" /> for all permission names. MeetingMaterial
    ///</summary>
    public class MeetingMaterialAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了MeetingMaterial 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var meetingmaterial = administration.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial , L("MeetingMaterials"));
meetingmaterial.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial_Create, L("Create"));
meetingmaterial.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial_Edit, L("Edit"));
meetingmaterial.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial_Delete, L("Delete"));
meetingmaterial.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial_BatchDelete , L("BatchDelete"));
meetingmaterial.CreateChildPermission(MeetingMaterialAppPermissions.MeetingMaterial_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }