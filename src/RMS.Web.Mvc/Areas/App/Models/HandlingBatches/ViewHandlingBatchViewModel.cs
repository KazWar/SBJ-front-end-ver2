using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;

namespace RMS.Web.Areas.App.Models.HandlingBatches
{
    public class ViewHandlingBatchViewModel
    {
        public GetPremiumBatchForView PremiumBatchForView { get; set; }

        public GetCashRefundBatchForView CashRefundBatchForView { get; set; }

        public GetActivationCodeBatchForView ActivationCodeBatchForView { get; set; }
    }
}
