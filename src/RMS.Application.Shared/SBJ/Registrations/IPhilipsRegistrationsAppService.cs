using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Registrations.Dtos;
using RMS.Dto;
using System.Collections.Generic;
using RMS.SBJ.Forms.Dtos;
using GetAllForLookupTableInput = RMS.SBJ.Registrations.Dtos.GetAllForLookupTableInput;

namespace RMS.SBJ.Registrations
{
	public interface IPhilipsRegistrationsAppService : IApplicationService
	{
		Task<PagedResultDto<GetRegistrationForViewDto>> GetAll(GetAllRegistrationsInput input);

		Task<GetRegistrationForViewDto> GetRegistrationForView(long id);

		Task<GetRegistrationForEditOutput> GetRegistrationForEdit(EntityDto<long> input);
		
		Task<GetEditForRegistrationDto> GetEditForRegistration(GetFormLayoutAndDataInput input);

		Task<GetRegistrationForProcessingOutput> GetRegistrationForProcessing(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRegistrationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRegistrationsToExcel(GetAllRegistrationsForExcelInput input);
		
		Task<PagedResultDto<RegistrationRegistrationStatusLookupTableDto>> GetAllRegistrationStatusForLookupTable(GetAllForLookupTableInput input);
		
		Task<List<RegistrationFormLocaleLookupTableDto>> GetAllFormLocaleForTableDropdown();

		Task<bool> SendFormData(string blobStorage, string blobContainer, PhilipsFormRegistrationHandlingDto vueJsToRmsModel);
	}
}