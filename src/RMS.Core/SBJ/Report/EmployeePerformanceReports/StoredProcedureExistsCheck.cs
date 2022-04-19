using Abp.Domain.Entities;

namespace RMS.SBJ.Report.EmployeePerformanceReports
{
    public class StoredProcedureExistsCheck : Entity<int>
    {
        public int Count { get; set; }
    }
}
