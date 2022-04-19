using RMS.MultiTenancy.Payments.Stripe;

namespace RMS.Web.Controllers
{
    public class StripeController : StripeControllerBase
    {
        public StripeController(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripeConfiguration,
            IStripePaymentAppService stripePaymentAppService) 
            : base(stripeGatewayManager, stripeConfiguration, stripePaymentAppService)
        {
        }
    }
}
