
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.VisitRecords.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="VisitRecordAppPermissions" /> for all permission names. VisitRecord
    ///</summary>
    public class VisitRecordAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了VisitRecord 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var visitrecord = administration.CreateChildPermission(VisitRecordAppPermissions.VisitRecord , L("VisitRecords"));
visitrecord.CreateChildPermission(VisitRecordAppPermissions.VisitRecord_Create, L("Create"));
visitrecord.CreateChildPermission(VisitRecordAppPermissions.VisitRecord_Edit, L("Edit"));
visitrecord.CreateChildPermission(VisitRecordAppPermissions.VisitRecord_Delete, L("Delete"));
visitrecord.CreateChildPermission(VisitRecordAppPermissions.VisitRecord_BatchDelete , L("BatchDelete"));
visitrecord.CreateChildPermission(VisitRecordAppPermissions.VisitRecord_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }