using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.HandlingLines.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.HandlingLines.Exporting
{
    public class HandlingLinesExcelExporter : NpoiExcelExporterBase, IHandlingLinesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HandlingLinesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHandlingLineForViewDto> handlingLines)
        {
            return CreateExcelPackage(
                "HandlingLines.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HandlingLines"));

                    AddHeader(
                        sheet,
                        L("MinimumPurchaseAmount"),
                        L("MaximumPurchaseAmount"),
                        L("CustomerCode"),
                        L("Amount"),
                        L("Fixed"),
                        L("ActivationCode"),
                        L("Quantity"),
                        (L("CampaignType")) + L("Name"),
                        (L("ProductHandling")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, handlingLines,
                        _ => _.HandlingLine.MinimumPurchaseAmount,
                        _ => _.HandlingLine.MaximumPurchaseAmount,
                        _ => _.HandlingLine.CustomerCode,
                        _ => _.HandlingLine.Amount,
                        _ => _.HandlingLine.Fixed,
                        _ => _.HandlingLine.ActivationCode,
                        _ => _.HandlingLine.Quantity,
                        _ => _.CampaignTypeName,
                        _ => _.ProductHandlingDescription
                        );

                });
        }
    }
}