
namespace GYISMS.MeetingMaterials.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MeetingMaterialAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class MeetingMaterialAppPermissions
    {
    /// <summary>
        /// MeetingMaterial管理权限_自带查询授权
        ///</summary>
    public const string MeetingMaterial = "Pages.MeetingMaterial";

    /// <summary>
        /// MeetingMaterial创建权限
        ///</summary>
    public const string MeetingMaterial_Create = "Pages.MeetingMaterial.Create";

    /// <summary>
        /// MeetingMaterial修改权限
        ///</summary>
    public const string MeetingMaterial_Edit = "Pages.MeetingMaterial.Edit";

    /// <summary>
        /// MeetingMaterial删除权限
        ///</summary>
    public const string MeetingMaterial_Delete = "Pages.MeetingMaterial.Delete";

    /// <summary>
        /// MeetingMaterial批量删除权限
        ///</summary>
    public const string MeetingMaterial_BatchDelete  = "Pages.MeetingMaterial.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string MeetingMaterial_ExportToExcel = "Pages.MeetingMaterial.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

