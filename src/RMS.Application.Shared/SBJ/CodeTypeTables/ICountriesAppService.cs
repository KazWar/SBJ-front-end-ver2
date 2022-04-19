using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.PromoPlanner.Dtos;
using System.Collections.Generic;

namespace RMS.SBJ.CodeTypeTables
{
    public interface ICountriesAppService : IApplicationService 
    {
        Task<IEnumerable<GetCountryForViewDto>> GetAll();

        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input);

        Task<IEnumerable<CustomPromoCountryForView>> GetAllWithoutPaging();

        Task<GetCountryForViewDto> GetCountryForView(long id);

		Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCountryDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input);	
    }
}