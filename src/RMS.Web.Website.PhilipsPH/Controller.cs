namespace RMS.Web.Website.PhilipsPH;
public class Controller : ApiController
{
    public Controller(
        IOptions<AuthenticationConfiguration> authenticationConfiguration,
        IOptions<TenantConfiguration> tenantConfiguration,
        IOptions<BuildConfiguration> buildConfiguration) : base(authenticationConfiguration, tenantConfiguration, buildConfiguration)
    {
    }
}