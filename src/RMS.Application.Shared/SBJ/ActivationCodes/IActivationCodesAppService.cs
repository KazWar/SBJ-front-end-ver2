using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.ActivationCodes.Dtos;
using RMS.Dto;


namespace RMS.SBJ.ActivationCodes
{
    public interface IActivationCodesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetActivationCodeForViewDto>> GetAll(GetAllActivationCodesInput input);

		Task<GetActivationCodeForEditOutput> GetActivationCodeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditActivationCodeDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<ActivationCodeLocaleLookupTableDto>> GetAllLocaleForLookupTable(GetAllForLookupTableInput input);
		
    }
}