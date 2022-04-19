using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.Dto;
using RMS.SBJ.Report.CostReports.Dtos;
using RMS.Storage;

namespace RMS.SBJ.Report.CostReports.Exporting
{
    public class CostReportExcelExporter : NpoiExcelExporterBase, ICostReportExcelExporter
    {
        public CostReportExcelExporter(
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager) { }

        public FileDto ExportToFile(string name, CostReportDto costReport)
        {
            return CreateExcelPackage(
                $"{name}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("CostReport"));

                    AddHeader(
                        sheet,
                        L("Year"),
                        L("Month"),
                        L("NewRegistrations"),
                        L("CompleteRegistrations"),
                        L("ExtraHandling"),
                        L("PaymentBatches"),
                        L("PaymentsSent"),
                        L("ActivationCodeBatches"),
                        L("ActivationCodesSent"),
                        L("PremiumBatches"),
                        L("PremiumsSent")
                        );

                    AddObjects(
                        sheet, 2, costReport.MonthlyTotals,
                        _ => _.Year,
                        _ => _.Month,
                        _ => _.NewRegistrations,
                        _ => _.CompleteRegistrations,
                        _ => _.ExtraHandlingRegistrations,
                        _ => _.PaymentBatches,
                        _ => _.PaymentsSent,
                        _ => _.ActivationCodeBatches,
                        _ => _.ActivationCodesSent,
                        _ => _.PremiumBatches,
                        _ => _.PremiumsSent
                        );

                    for (var j = 0; j <= 11; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                });
        }
    }
}
