using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.Forms
{
    public interface IPhilipsFormLocalesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetFormLocaleForViewDto>> GetAll(GetAllFormLocalesInput input);

        Task<GetFormLocaleForViewDto> GetFormLocaleForView(long id);

		Task<GetFormLocaleForEditOutput> GetFormLocaleForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditFormLocaleDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetFormLocalesToExcel(GetAllFormLocalesForExcelInput input);
		
		Task<PagedResultDto<FormLocaleFormLookupTableDto>> GetAllFormForLookupTable(GetAllForLookupTableInput input);

		Task<List<FormLocaleLocaleLookupTableDto>> GetAllLocaleForTableDropdown();

		Task<List<GetFormLocaleForViewDto>> GetAllFormLocales();

		Task<long> CreateOrEditAndGetId(CreateOrEditFormLocaleDto input);

		Task<GetFormLayoutAndDataDto> GetFormLayoutAndDataForRegistration(GetFormLayoutAndDataInput input);

		Task<GetFormAndProductHandelingDto> GetFormAndProductHandeling(long currentCampaignId, string currentLocale);

		Task<GetFormAndProductHandelingDto> GetEditFormHandling(long currentRegistrationId);
	}
} 