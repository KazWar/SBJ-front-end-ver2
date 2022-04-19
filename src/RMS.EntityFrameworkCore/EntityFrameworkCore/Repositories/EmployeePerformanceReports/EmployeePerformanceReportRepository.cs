using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.Report.EmployeePerformanceReports;

namespace RMS.EntityFrameworkCore.Repositories.EmployeePerformanceReports
{
    public class EmployeePerformanceReportRepository : RMSRepositoryBase<EmployeePerformanceReport, long>, IEmployeePerformanceReportRepository
    {
        public EmployeePerformanceReportRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> CheckStoredProcedureExistence()
        {
            var dbContext = (RMSDbContext)base.GetDbContext();
            var storedProcedureExistsCheck = await dbContext.StoredProcedureExistsCheck
                                                .FromSqlRaw("SELECT object_id AS Id, COUNT(*) AS Count " +
                                                "FROM sys.objects " +
                                                "WHERE type = 'P' AND name = 'EmployeePerformanceReport' " +
                                                "GROUP BY object_id").ToListAsync();

            return (storedProcedureExistsCheck.Count > 0);
        }

        public async Task<List<EmployeePerformanceReport>> GeneratePerformanceReport(DateTime fromDate, DateTime endDate)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            var employeePerformanceReport = dbContext.EmployeePerformanceReport
                                             .FromSqlInterpolated($"EXECUTE dbo.EmployeePerformanceReport {fromDate}, {endDate}");

            return await employeePerformanceReport.ToListAsync();
        }


    }
}
