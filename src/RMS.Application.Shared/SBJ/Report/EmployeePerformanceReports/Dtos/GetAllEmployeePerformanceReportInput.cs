using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Report.EmployeePerformanceReports.Dtos
{
    public class GetAllEmployeePerformanceReportInput : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDateFilter { get; set; }

        public DateTime? EndDateFilter { get; set; }
    }
}
