﻿using System.Collections.Generic;
using RMS.Editions;
using RMS.Editions.Dto;
using RMS.MultiTenancy.Payments;
using RMS.MultiTenancy.Payments.Dto;

namespace RMS.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
