
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.VisitExamines.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="VisitExamineAppPermissions" /> for all permission names. VisitExamine
    ///</summary>
    public class VisitExamineAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了VisitExamine 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var visitexamine = administration.CreateChildPermission(VisitExamineAppPermissions.VisitExamine , L("VisitExamines"));
visitexamine.CreateChildPermission(VisitExamineAppPermissions.VisitExamine_Create, L("Create"));
visitexamine.CreateChildPermission(VisitExamineAppPermissions.VisitExamine_Edit, L("Edit"));
visitexamine.CreateChildPermission(VisitExamineAppPermissions.VisitExamine_Delete, L("Delete"));
visitexamine.CreateChildPermission(VisitExamineAppPermissions.VisitExamine_BatchDelete , L("BatchDelete"));
visitexamine.CreateChildPermission(VisitExamineAppPermissions.VisitExamine_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }