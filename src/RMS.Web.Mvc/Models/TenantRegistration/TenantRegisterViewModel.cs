using RMS.Editions;
using RMS.Editions.Dto;
using RMS.MultiTenancy.Payments;
using RMS.Security;
using RMS.MultiTenancy.Payments.Dto;

namespace RMS.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
