namespace RMS.SBJ.HandlingBatch.Dtos.CashRefund
{
    public sealed class CashRefundBatchRegistrationForView
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string Postal { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string CampaignName { get; set; }

        public string StatusDescription { get; set; }

        public decimal RefundAmount { get; set; }
        
        public string Bic { get; set; }

        public string Iban { get; set; }
    }
}
