using Abp.Application.Services;
using System.Threading.Tasks;
using RMS.SBJ.Report.EmployeePerformanceReports.Dtos;
using Abp.Application.Services.Dto;
using RMS.Dto;

namespace RMS.SBJ.Report.EmployeePerformanceReports
{
    public interface IEmployeePerformanceReportAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeePerformanceReportForViewDto>> GetAll(GetAllEmployeePerformanceReportInput input);

        Task<FileDto> GetEmployeePerformanceReportToExcel(GetAllEmployeePerformanceReportForExcelInput input);

        bool CheckIfStoredProcedureExists();
    }
}
