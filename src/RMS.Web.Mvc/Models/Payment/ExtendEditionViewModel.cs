using System.Collections.Generic;
using RMS.Editions.Dto;
using RMS.MultiTenancy.Payments;

namespace RMS.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}