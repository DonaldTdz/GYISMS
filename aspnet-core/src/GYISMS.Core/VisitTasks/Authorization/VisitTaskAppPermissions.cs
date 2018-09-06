
namespace GYISMS.VisitTasks.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="VisitTaskAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class VisitTaskAppPermissions
    {
    /// <summary>
        /// VisitTask管理权限_自带查询授权
        ///</summary>
    public const string VisitTask = "Pages.VisitTask";

    /// <summary>
        /// VisitTask创建权限
        ///</summary>
    public const string VisitTask_Create = "Pages.VisitTask.Create";

    /// <summary>
        /// VisitTask修改权限
        ///</summary>
    public const string VisitTask_Edit = "Pages.VisitTask.Edit";

    /// <summary>
        /// VisitTask删除权限
        ///</summary>
    public const string VisitTask_Delete = "Pages.VisitTask.Delete";

    /// <summary>
        /// VisitTask批量删除权限
        ///</summary>
    public const string VisitTask_BatchDelete  = "Pages.VisitTask.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string VisitTask_ExportToExcel = "Pages.VisitTask.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

