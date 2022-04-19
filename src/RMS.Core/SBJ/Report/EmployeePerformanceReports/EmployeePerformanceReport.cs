using Abp.Domain.Entities;
using System;

namespace RMS.SBJ.Report.EmployeePerformanceReports
{
    public class EmployeePerformanceReport : Entity<long>
    {
        public String UserName { get; set; }

        public DateTime Datum { get; set; }

        public int Aantal { get; set; }
    }
}
