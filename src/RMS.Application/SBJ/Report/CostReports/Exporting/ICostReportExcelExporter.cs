using RMS.Dto;
using RMS.SBJ.Report.CostReports.Dtos;

namespace RMS.SBJ.Report.CostReports.Exporting
{
    public interface ICostReportExcelExporter
    {
        FileDto ExportToFile(string name, CostReportDto getCostReports);
    }
}
