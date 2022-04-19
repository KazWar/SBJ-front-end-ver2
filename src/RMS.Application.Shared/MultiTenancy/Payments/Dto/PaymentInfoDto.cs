﻿using RMS.Editions.Dto;

namespace RMS.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < RMSConsts.MinimumUpgradePaymentAmount;
        }
    }
}
