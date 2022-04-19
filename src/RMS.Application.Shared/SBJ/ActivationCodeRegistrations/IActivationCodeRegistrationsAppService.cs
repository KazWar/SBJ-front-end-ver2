using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.ActivationCodeRegistrations.Dtos;
using RMS.Dto;


namespace RMS.SBJ.ActivationCodeRegistrations
{
    public interface IActivationCodeRegistrationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetActivationCodeRegistrationForViewDto>> GetAll(GetAllActivationCodeRegistrationsInput input);

		Task<GetActivationCodeRegistrationForEditOutput> GetActivationCodeRegistrationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditActivationCodeRegistrationDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<ActivationCodeRegistrationActivationCodeLookupTableDto>> GetAllActivationCodeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ActivationCodeRegistrationRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input);
		
    }
}