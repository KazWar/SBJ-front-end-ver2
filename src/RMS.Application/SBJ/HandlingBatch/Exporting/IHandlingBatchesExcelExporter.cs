using System.Collections.Generic;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;
using RMS.Dto;

namespace RMS.SBJ.HandlingBatch.Exporting
{
    public interface IHandlingBatchesExcelExporter
    {
        FileDto ExportToFile(List<GetHandlingBatchForViewDto> handlingBatches);

        FileDto ExportToFilePM(long id, List<PremiumBatchRegistrationForView> exportLines);

        FileDto ExportToFileCR(long id, List<CashRefundBatchRegistrationForView> exportLines);

        FileDto ExportToFileAC(long id, List<ActivationCodeBatchRegistrationForView> exportLines);
    }
}