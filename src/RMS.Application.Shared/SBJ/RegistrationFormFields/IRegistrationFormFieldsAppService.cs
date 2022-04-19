using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.RegistrationFormFields.Dtos;
using RMS.Dto;


namespace RMS.SBJ.RegistrationFormFields
{
    public interface IRegistrationFormFieldsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRegistrationFormFieldForViewDto>> GetAll(GetAllRegistrationFormFieldsInput input);

		Task<GetRegistrationFormFieldForEditOutput> GetRegistrationFormFieldForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRegistrationFormFieldDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<RegistrationFormFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
    }
}