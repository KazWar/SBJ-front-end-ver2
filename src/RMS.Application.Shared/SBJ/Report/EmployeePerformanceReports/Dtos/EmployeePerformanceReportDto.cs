using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.Report.EmployeePerformanceReports.Dtos
{
    public class EmployeePerformanceReportDto : EntityDto<long>
    {
        public String UserName { get; set; }

        public DateTime Datum { get; set; }

        public int Aantal { get; set; }
    }
}
