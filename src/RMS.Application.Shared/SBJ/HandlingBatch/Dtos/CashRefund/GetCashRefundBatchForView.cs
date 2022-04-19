using System;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingBatch.Dtos.CashRefund
{
    public sealed class GetCashRefundBatchForView
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RegistrationsCount { get; set; }

        public decimal TotalRefundAmount { get; set; }

        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string Remarks { get; set; }

        public string SepaInitiator { get; set; }

        public IEnumerable<CashRefundBatchCampaignForView> Campaigns { get; set; }
    }
}
