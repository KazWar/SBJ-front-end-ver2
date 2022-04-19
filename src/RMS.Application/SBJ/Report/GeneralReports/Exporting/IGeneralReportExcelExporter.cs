using RMS.Dto;
using RMS.SBJ.Report.GeneralReports.Dtos;
using System.Collections.Generic;

namespace RMS.SBJ.Report.GeneralReports.Exporting
{
    public interface IGeneralReportExcelExporter
    {
        FileDto ExportToFile(string name, List<GeneralReportDto> getGeneralReports);
    }
}
