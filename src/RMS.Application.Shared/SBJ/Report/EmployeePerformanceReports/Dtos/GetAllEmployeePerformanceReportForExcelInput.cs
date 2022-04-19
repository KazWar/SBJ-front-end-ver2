using System;

namespace RMS.SBJ.Report.EmployeePerformanceReports.Dtos
{
    public class GetAllEmployeePerformanceReportForExcelInput
    {
        public DateTime? StartDateFilter { get; set; }

        public DateTime? EndDateFilter { get; set; }
    }
}
