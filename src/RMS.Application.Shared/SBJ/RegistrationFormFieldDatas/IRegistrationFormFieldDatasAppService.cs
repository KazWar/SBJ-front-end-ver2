using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.RegistrationFormFieldDatas.Dtos;
using RMS.Dto;


namespace RMS.SBJ.RegistrationFormFieldDatas
{
    public interface IRegistrationFormFieldDatasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRegistrationFormFieldDataForViewDto>> GetAll(GetAllRegistrationFormFieldDatasInput input);

		Task<GetRegistrationFormFieldDataForEditOutput> GetRegistrationFormFieldDataForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRegistrationFormFieldDataDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<RegistrationFormFieldDataRegistrationFormFieldLookupTableDto>> GetAllRegistrationFormFieldForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RegistrationFormFieldDataRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input);
		
    }
}