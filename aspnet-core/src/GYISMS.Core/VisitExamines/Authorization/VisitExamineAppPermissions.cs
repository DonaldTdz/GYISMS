
namespace GYISMS.VisitExamines.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="VisitExamineAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class VisitExamineAppPermissions
    {
    /// <summary>
        /// VisitExamine管理权限_自带查询授权
        ///</summary>
    public const string VisitExamine = "Pages.VisitExamine";

    /// <summary>
        /// VisitExamine创建权限
        ///</summary>
    public const string VisitExamine_Create = "Pages.VisitExamine.Create";

    /// <summary>
        /// VisitExamine修改权限
        ///</summary>
    public const string VisitExamine_Edit = "Pages.VisitExamine.Edit";

    /// <summary>
        /// VisitExamine删除权限
        ///</summary>
    public const string VisitExamine_Delete = "Pages.VisitExamine.Delete";

    /// <summary>
        /// VisitExamine批量删除权限
        ///</summary>
    public const string VisitExamine_BatchDelete  = "Pages.VisitExamine.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string VisitExamine_ExportToExcel = "Pages.VisitExamine.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

