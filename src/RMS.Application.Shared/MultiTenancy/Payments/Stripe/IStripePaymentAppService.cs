using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.MultiTenancy.Payments.Dto;
using RMS.MultiTenancy.Payments.Stripe.Dto;

namespace RMS.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}