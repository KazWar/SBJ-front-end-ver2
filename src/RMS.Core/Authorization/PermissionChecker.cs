using Abp.Authorization;
using RMS.Authorization.Roles;
using RMS.Authorization.Users;

namespace RMS.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
