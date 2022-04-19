using Abp.Domain.Services;

namespace RMS
{
    public abstract class RMSDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected RMSDomainServiceBase()
        {
            LocalizationSourceName = RMSConsts.LocalizationSourceName;
        }
    }
}
