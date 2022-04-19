using System.Threading.Tasks;
using Abp.Application.Services;
using RMS.MultiTenancy.Payments.PayPal.Dto;

namespace RMS.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
