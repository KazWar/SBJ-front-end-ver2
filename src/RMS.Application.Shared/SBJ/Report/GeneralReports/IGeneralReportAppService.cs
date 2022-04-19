using Abp.Application.Services;
using RMS.Dto;
using RMS.SBJ.Report.GeneralReports.Dtos;
using System.Threading.Tasks;

namespace RMS.SBJ.Report.GeneralReports
{
    public interface IGeneralReportAppService : IApplicationService
    {
        Task<FileDto> GetGeneralReportToExcel(GetAllGeneralReportForExcelInput input);
    }
}
