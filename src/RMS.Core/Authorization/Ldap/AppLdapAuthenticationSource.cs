using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using RMS.Authorization.Users;
using RMS.MultiTenancy;

namespace RMS.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}