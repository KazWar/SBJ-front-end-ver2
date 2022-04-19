using Abp.Application.Services.Dto;
using Abp.Authorization;
using RMS.Authorization;
using RMS.Dto;
using RMS.EntityFrameworkCore.Repositories.EmployeePerformanceReports;
using RMS.SBJ.Report.EmployeePerformanceReports.Dtos;
using RMS.SBJ.Report.EmployeePerformanceReports.Exporting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.SBJ.Report.EmployeePerformanceReports
{
    [AbpAuthorize(AppPermissions.Pages_EmployeePerformanceReport)]
    public class EmployeePerformanceReportAppService : RMSAppServiceBase, IEmployeePerformanceReportAppService
    {
        private readonly IEmployeePerformanceReportRepository _employeePerformanceReportRepository;
        private readonly IEmployeePerformanceReportExcelExporter _employeePerformanceReportExcelExporter;

        public EmployeePerformanceReportAppService(
            IEmployeePerformanceReportRepository employeePerformanceReportRepository,
            IEmployeePerformanceReportExcelExporter employeePerformanceReportExcelExporter)
        {
            _employeePerformanceReportRepository = employeePerformanceReportRepository;
            _employeePerformanceReportExcelExporter = employeePerformanceReportExcelExporter;
        }

        public async Task<PagedResultDto<GetEmployeePerformanceReportForViewDto>> GetAll(GetAllEmployeePerformanceReportInput input)
        {
            var _getEmployeePerformanceReportForViewDtoList = new List<GetEmployeePerformanceReportForViewDto>();

            var _data = _employeePerformanceReportRepository.GeneratePerformanceReport((DateTime)input.StartDateFilter, (DateTime)input.EndDateFilter).Result;
            
            foreach (EmployeePerformanceReport employeePerformanceReport in _data)
            {
                _getEmployeePerformanceReportForViewDtoList.Add(new GetEmployeePerformanceReportForViewDto()
                {
                    EmployeePerformanceReport = ObjectMapper.Map<EmployeePerformanceReportDto>(employeePerformanceReport)
                });
            }

            return new PagedResultDto<GetEmployeePerformanceReportForViewDto>(
                _getEmployeePerformanceReportForViewDtoList.Count,
                _getEmployeePerformanceReportForViewDtoList
            );
        }

        public async Task<FileDto> GetEmployeePerformanceReportToExcel(GetAllEmployeePerformanceReportForExcelInput input)
        {
            var _getEmployeePerformanceReportForViewDtoList = new List<GetEmployeePerformanceReportForViewDto>();

            var _data = _employeePerformanceReportRepository.GeneratePerformanceReport((DateTime)input.StartDateFilter, (DateTime)input.EndDateFilter).Result;

            foreach (EmployeePerformanceReport employeePerformanceReport in _data)
            {
                _getEmployeePerformanceReportForViewDtoList.Add(new GetEmployeePerformanceReportForViewDto()
                {
                    EmployeePerformanceReport = ObjectMapper.Map<EmployeePerformanceReportDto>(employeePerformanceReport)
                }) ;
            }

            return _employeePerformanceReportExcelExporter.ExportToFile(_getEmployeePerformanceReportForViewDtoList);
        }

        public bool CheckIfStoredProcedureExists()
        {
            return _employeePerformanceReportRepository.CheckStoredProcedureExistence().Result;
        }
    }
}
