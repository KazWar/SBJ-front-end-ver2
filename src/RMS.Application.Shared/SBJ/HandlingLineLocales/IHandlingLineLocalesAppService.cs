using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingLineLocales.Dtos;
using RMS.Dto;


namespace RMS.SBJ.HandlingLineLocales
{
    public interface IHandlingLineLocalesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHandlingLineLocaleForViewDto>> GetAll(GetAllHandlingLineLocalesInput input);

		Task<GetHandlingLineLocaleForEditOutput> GetHandlingLineLocaleForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHandlingLineLocaleDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<HandlingLineLocaleHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<HandlingLineLocaleLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input);
		
    }
}