using System.Threading.Tasks;
using Abp.Application.Services;

namespace RMS.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
