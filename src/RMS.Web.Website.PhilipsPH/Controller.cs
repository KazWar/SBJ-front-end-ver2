using RMS.Web.Shared.Configuration;
using RMS.Web.Shared.Controllers;

namespace RMS.Web.Website.PhilipsPH;
public class Controller
{
    private readonly AuthenticationConfiguration AuthenticationConfig;
    private readonly TenantConfiguration TenantConfiguration;
    private readonly BuildConfiguration BuildConfiguration;
    private readonly ApiController ApiController;

    public Controller(IConfiguration configuration)
    {
        // Bind the appsettings.json configurations
        AuthenticationConfig = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();
        TenantConfiguration = configuration.GetSection("Tenant").Get<TenantConfiguration>();
        BuildConfiguration = configuration.GetSection("Build").Get<BuildConfiguration>();

        // Create an instance of the shared API controller
        ApiController = new ApiController(
            AuthenticationConfig,
            TenantConfiguration,
            BuildConfiguration
        );

        // Start the API controller
        ApiController.Init();
    }
}