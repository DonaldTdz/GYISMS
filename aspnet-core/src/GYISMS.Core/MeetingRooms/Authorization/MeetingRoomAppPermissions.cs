
namespace GYISMS.MeetingRooms.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MeetingRoomAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class MeetingRoomAppPermissions
    {
    /// <summary>
        /// MeetingRoom管理权限_自带查询授权
        ///</summary>
    public const string MeetingRoom = "Pages.MeetingRoom";

    /// <summary>
        /// MeetingRoom创建权限
        ///</summary>
    public const string MeetingRoom_Create = "Pages.MeetingRoom.Create";

    /// <summary>
        /// MeetingRoom修改权限
        ///</summary>
    public const string MeetingRoom_Edit = "Pages.MeetingRoom.Edit";

    /// <summary>
        /// MeetingRoom删除权限
        ///</summary>
    public const string MeetingRoom_Delete = "Pages.MeetingRoom.Delete";

    /// <summary>
        /// MeetingRoom批量删除权限
        ///</summary>
    public const string MeetingRoom_BatchDelete  = "Pages.MeetingRoom.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string MeetingRoom_ExportToExcel = "Pages.MeetingRoom.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

