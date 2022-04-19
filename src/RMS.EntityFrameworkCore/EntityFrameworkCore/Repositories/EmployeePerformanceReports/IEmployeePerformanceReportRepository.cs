using Abp.Domain.Repositories;
using RMS.SBJ.Report.EmployeePerformanceReports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.EmployeePerformanceReports
{
    public interface IEmployeePerformanceReportRepository : IRepository<EmployeePerformanceReport, long>
    {
        Task<List<EmployeePerformanceReport>> GeneratePerformanceReport(DateTime fromDate, DateTime endDate);

        Task<bool> CheckStoredProcedureExistence();
    }
}
