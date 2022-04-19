using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using System.Collections.Generic;


namespace RMS.SBJ.CodeTypeTables
{
    public interface ILocalesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLocaleForViewDto>> GetAll(GetAllLocalesInput input);

        Task<GetLocaleForViewDto> GetLocaleForView(long id);

		Task<GetLocaleForEditOutput> GetLocaleForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditLocaleDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetLocalesToExcel(GetAllLocalesForExcelInput input);
		
		Task<List<LocaleCountryLookupTableDto>> GetAllCountryForTableDropdown();

		Task<List<GetLocaleForViewDto>> GetAllLocales();

		Task<List<GetLocaleForViewDto>> GetAllLocalesOnCompanyLevel();
	}
}