
namespace GYISMS.ScheduleDetails.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="ScheduleDetailAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class ScheduleDetailAppPermissions
    {
    /// <summary>
        /// ScheduleDetail管理权限_自带查询授权
        ///</summary>
    public const string ScheduleDetail = "Pages.ScheduleDetail";

    /// <summary>
        /// ScheduleDetail创建权限
        ///</summary>
    public const string ScheduleDetail_Create = "Pages.ScheduleDetail.Create";

    /// <summary>
        /// ScheduleDetail修改权限
        ///</summary>
    public const string ScheduleDetail_Edit = "Pages.ScheduleDetail.Edit";

    /// <summary>
        /// ScheduleDetail删除权限
        ///</summary>
    public const string ScheduleDetail_Delete = "Pages.ScheduleDetail.Delete";

    /// <summary>
        /// ScheduleDetail批量删除权限
        ///</summary>
    public const string ScheduleDetail_BatchDelete  = "Pages.ScheduleDetail.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string ScheduleDetail_ExportToExcel = "Pages.ScheduleDetail.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

