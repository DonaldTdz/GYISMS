
namespace GYISMS.Growers.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="GrowerAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class GrowerAppPermissions
    {
    /// <summary>
        /// Grower管理权限_自带查询授权
        ///</summary>
    public const string Grower = "Pages.Grower";

    /// <summary>
        /// Grower创建权限
        ///</summary>
    public const string Grower_Create = "Pages.Grower.Create";

    /// <summary>
        /// Grower修改权限
        ///</summary>
    public const string Grower_Edit = "Pages.Grower.Edit";

    /// <summary>
        /// Grower删除权限
        ///</summary>
    public const string Grower_Delete = "Pages.Grower.Delete";

    /// <summary>
        /// Grower批量删除权限
        ///</summary>
    public const string Grower_BatchDelete  = "Pages.Grower.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Grower_ExportToExcel = "Pages.Grower.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

