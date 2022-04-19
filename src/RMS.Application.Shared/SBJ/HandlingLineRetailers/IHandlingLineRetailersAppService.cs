using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingLineRetailers.Dtos;
using RMS.Dto;


namespace RMS.SBJ.HandlingLineRetailers
{
    public interface IHandlingLineRetailersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHandlingLineRetailerForViewDto>> GetAll(GetAllHandlingLineRetailersInput input);

		Task<GetHandlingLineRetailerForEditOutput> GetHandlingLineRetailerForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHandlingLineRetailerDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<HandlingLineRetailerHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<HandlingLineRetailerRetailerLookupTableDto>> GetAllRetailerForLookupTable(GetAllForLookupTableInput input);
		
    }
}