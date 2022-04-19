using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.Dto;
using RMS.SBJ.Report.EmployeePerformanceReports.Dtos;
using RMS.Storage;
using System.Collections.Generic;

namespace RMS.SBJ.Report.EmployeePerformanceReports.Exporting
{
    public class EmployeePerformanceReportExcelExporter : NpoiExcelExporterBase, IEmployeePerformanceReportExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeePerformanceReportExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeePerformanceReportForViewDto> employeePerformanceReport)
        {
            return CreateExcelPackage(
                "EmployeePerformanceReport.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("EmployeePerformanceReport"));

                    AddHeader(
                        sheet,
                        L("AbpUserId"),
                        L("UserName"),
                        L("Date"),
                        L("Count")
                        );

                    AddObjects(
                        sheet, 2, employeePerformanceReport,
                        _ => _.EmployeePerformanceReport.Id,
                        _ => _.EmployeePerformanceReport.UserName.Replace('.', ' '),
                        _ => _timeZoneConverter.Convert(_.EmployeePerformanceReport.Datum, _abpSession.TenantId, _abpSession.GetUserId()).Value.ToShortDateString(),
                        _ => _.EmployeePerformanceReport.Aantal
                        );


                    for (var i = 1; i <= employeePerformanceReport.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}
