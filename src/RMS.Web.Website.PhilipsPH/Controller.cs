using RMS.Web.Shared.Controllers;
using RMS.Web.Shared.Configuration;

namespace RMS.Web.Website.PhilipsPH;
public class Controller
{
    private readonly AuthenticationConfiguration AuthenticationConfig;
    private readonly TenantConfiguration TenantConfiguration;
    private readonly BuildConfiguration BuildConfiguration;
    private readonly ApiConfiguration ApiConfiguration;
    private readonly PostCodeApi PostCodeApiConfiguration;
    private readonly IbanRechnerConfiguration IbanRechnerConfiguration;
    private readonly ApiController ApiController;

    public Controller(IConfiguration configuration)
    {
        AuthenticationConfig = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();
        TenantConfiguration = configuration.GetSection("Tenant").Get<TenantConfiguration>();
        ApiConfiguration = configuration.GetSection("Api").Get<ApiConfiguration>();
        BuildConfiguration = configuration.GetSection("Build").Get<BuildConfiguration>();
        PostCodeApiConfiguration = configuration.GetSection("PostCodeApi").Get<PostCodeApi>();
        IbanRechnerConfiguration = configuration.GetSection("IbanRechner").Get<IbanRechnerConfiguration>();

        ApiController = new ApiController(
            AuthenticationConfig,
            TenantConfiguration,
            BuildConfiguration,
            ApiConfiguration,
            PostCodeApiConfiguration,
            IbanRechnerConfiguration
        );

        ApiController.Init();
    }
}