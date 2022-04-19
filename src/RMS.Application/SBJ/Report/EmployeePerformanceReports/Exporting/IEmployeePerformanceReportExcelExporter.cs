using RMS.Dto;
using RMS.SBJ.Report.EmployeePerformanceReports.Dtos;
using System.Collections.Generic;

namespace RMS.SBJ.Report.EmployeePerformanceReports.Exporting
{
    public interface IEmployeePerformanceReportExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeePerformanceReportForViewDto> getEmployeePerformanceReports);
    }
}
