using Abp.Application.Services;
using RMS.Dto;
using RMS.SBJ.Report.CostReports.Dtos;
using System.Threading.Tasks;

namespace RMS.SBJ.Report.CostReports
{
    public interface ICostReportAppService : IApplicationService
    {
        Task<CostReportDto> GetCostReport(GetAllCostReportInput input);
        Task<FileDto> GetCostReportToExcel(GetAllCostReportInput input);
    }
}
