using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.Dto;
using RMS.Storage;
using System.Threading;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;

namespace RMS.SBJ.HandlingBatch.Exporting
{
    public class HandlingBatchesExcelExporter : NpoiExcelExporterBase, IHandlingBatchesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HandlingBatchesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHandlingBatchForViewDto> handlingBatches)
        {
            return CreateExcelPackage(
                "HandlingBatches.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("HandlingBatches"));

                    AddHeader(
                        sheet,
                        L("DateCreated"),
                        L("Remarks"),
                        (L("CampaignType")) + L("Name"),
                        (L("HandlingBatchStatus")) + L("StatusDescription")
                        );

                    AddObjects(
                        sheet, 2, handlingBatches,
                        _ => _timeZoneConverter.Convert(_.HandlingBatch.DateCreated, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.HandlingBatch.Remarks,
                        _ => _.CampaignTypeName,
                        _ => _.HandlingBatchStatusStatusDescription
                        );

                    for (var i = 1; i <= handlingBatches.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);
                });
        }

        public FileDto ExportToFileCR(long id, List<CashRefundBatchRegistrationForView> exportLines)
        {
            return CreateExcelPackage(
                $"CashRefundBatch_{id}.xlsx",
                excelPackage =>
                {                   
                    var sheet = excelPackage.CreateSheet($"CashRefundBatch_{id}");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl-NL");

                    AddHeader(
                        sheet,
                        "CampaignName",
                        "RegistrationId",
                        "Name",
                        "Street",
                        "Postal",
                        "City",
                        "Country",
                        "Email",
                        "RefundAmount",
                        "Bic",
                        "Iban"
                        );

                    AddObjects(
                        sheet, 2, exportLines,
                        _ => _.CampaignName,
                        _ => _.Id,
                        _ => _.Name,
                        _ => _.Street,
                        _ => _.Postal,
                        _ => _.City,
                        _ => _.Country,
                        _ => _.Email,
                        _ => _.RefundAmount.ToString().Replace('.',','),
                        _ => _.Bic,
                        _ => _.Iban
                        );

                    //for (var i = 1; i <= exportLines.Count; i++)
                    //{
                    //    SetCellDataFormat(sheet.GetRow(i).Cells[5], "0.00");
                    //    var cellStyle = sheet.Workbook.CreateCellStyle();
                    //    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    //    sheet.GetRow(i).Cells[5].CellStyle = cellStyle;
                    //    sheet.GetRow(i).Cells[5].SetCellType(CellType.Numeric);
                    //}

                    for (var j = 0; j <= 10; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                });
        }

        public FileDto ExportToFilePM(long id, List<PremiumBatchRegistrationForView> exportLines)
        {
            return CreateExcelPackage(
                $"PremiumBatch_{id}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet($"PremiumBatch_{id}");

                    AddHeader(
                        sheet,
                        "CampaignName",
                        "RegistrationId",
                        "Name",
                        "Street",
                        "Postal",
                        "City",
                        "Country",
                        "Email",
                        "Premium(s)",
                        "WMSOrderId",
                        "WMSOrderStatus"
                        );

                    AddObjects(
                        sheet, 2, exportLines,
                        _ => _.CampaignName,
                        _ => _.Id,
                        _ => _.Name,
                        _ => _.Street,
                        _ => _.Postal,
                        _ => _.City,
                        _ => _.Country,
                        _ => _.Email,
                        _ => _.PremiumDescription,
                        _ => _.OrderId,
                        _ => _.OrderStatus 
                        );

                    for (var j = 0; j <= 10; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                });
        }

        public FileDto ExportToFileAC(long id, List<ActivationCodeBatchRegistrationForView> exportLines)
        {
            return CreateExcelPackage(
                $"ActivationCodeBatch_{id}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet($"ActivationCodeBatch_{id}");

                    AddHeader(
                        sheet,
                        "CampaignName",
                        "RegistrationId",
                        "Name",
                        "Street",
                        "Postal",
                        "City",
                        "Country",
                        "Email",
                        "ActivationCode(s)",
                        "MessageStatus"
                        );

                    AddObjects(
                        sheet, 2, exportLines,
                        _ => _.CampaignName,
                        _ => _.Id,
                        _ => _.Name,
                        _ => _.Street,
                        _ => _.Postal,
                        _ => _.City,
                        _ => _.Country,
                        _ => _.Email,
                        _ => _.ActivationCode,
                        _ => _.MessageStatus 
                        );

                    for (var j = 0; j <= 9; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                });
        }
    }
}