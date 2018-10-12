using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using GYISMS.GYEnums;
using GYISMS.Interfaces;

namespace GYISMS.Authorization.Users
{
    public class User : AbpUser<User> , IMayArea
    {
        public const string DefaultPassword = "123qwe";
        public virtual string Area { get; set; }
        public virtual AreaCodeEnum? AreaCode { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
