using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Company
{
    public interface ICompaniesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input);

        Task<GetCompanyForViewDto> GetCompanyForView(long id);

		Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCompanyDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCompaniesToExcel(GetAllCompaniesForExcelInput input);

		
		Task<PagedResultDto<CompanyAddressLookupTableDto>> GetAllAddressForLookupTable(GetAllForLookupTableInput input);
		
    }
}