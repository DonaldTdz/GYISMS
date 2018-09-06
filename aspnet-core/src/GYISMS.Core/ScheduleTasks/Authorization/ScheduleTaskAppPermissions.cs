
namespace GYISMS.ScheduleTasks.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="ScheduleTaskAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class ScheduleTaskAppPermissions
    {
    /// <summary>
        /// ScheduleTask管理权限_自带查询授权
        ///</summary>
    public const string ScheduleTask = "Pages.ScheduleTask";

    /// <summary>
        /// ScheduleTask创建权限
        ///</summary>
    public const string ScheduleTask_Create = "Pages.ScheduleTask.Create";

    /// <summary>
        /// ScheduleTask修改权限
        ///</summary>
    public const string ScheduleTask_Edit = "Pages.ScheduleTask.Edit";

    /// <summary>
        /// ScheduleTask删除权限
        ///</summary>
    public const string ScheduleTask_Delete = "Pages.ScheduleTask.Delete";

    /// <summary>
        /// ScheduleTask批量删除权限
        ///</summary>
    public const string ScheduleTask_BatchDelete  = "Pages.ScheduleTask.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string ScheduleTask_ExportToExcel = "Pages.ScheduleTask.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

