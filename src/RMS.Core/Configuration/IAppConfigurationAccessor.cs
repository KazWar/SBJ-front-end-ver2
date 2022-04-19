using Microsoft.Extensions.Configuration;

namespace RMS.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
