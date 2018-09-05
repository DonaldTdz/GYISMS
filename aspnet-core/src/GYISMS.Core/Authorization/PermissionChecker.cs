using Abp.Authorization;
using GYISMS.Authorization.Roles;
using GYISMS.Authorization.Users;

namespace GYISMS.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
