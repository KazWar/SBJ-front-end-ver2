using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;

namespace RMS.Web.Areas.App.Models.HandlingBatches
{
    public class NewHandlingBatchViewModel
    {
        public GetInformationForNewPremiumBatchOutput InformationForNewPremiumBatch { get; set; }

        public GetInformationForNewCashRefundBatchOutput InformationForNewCashRefundBatch { get; set; }

        public GetInformationForNewActivationCodeBatchOutput InformationForNewActivationCodeBatch { get; set; }
    }
}
