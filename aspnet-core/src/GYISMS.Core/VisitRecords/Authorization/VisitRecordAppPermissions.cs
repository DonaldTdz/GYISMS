
namespace GYISMS.VisitRecords.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="VisitRecordAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class VisitRecordAppPermissions
    {
    /// <summary>
        /// VisitRecord管理权限_自带查询授权
        ///</summary>
    public const string VisitRecord = "Pages.VisitRecord";

    /// <summary>
        /// VisitRecord创建权限
        ///</summary>
    public const string VisitRecord_Create = "Pages.VisitRecord.Create";

    /// <summary>
        /// VisitRecord修改权限
        ///</summary>
    public const string VisitRecord_Edit = "Pages.VisitRecord.Edit";

    /// <summary>
        /// VisitRecord删除权限
        ///</summary>
    public const string VisitRecord_Delete = "Pages.VisitRecord.Delete";

    /// <summary>
        /// VisitRecord批量删除权限
        ///</summary>
    public const string VisitRecord_BatchDelete  = "Pages.VisitRecord.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string VisitRecord_ExportToExcel = "Pages.VisitRecord.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

