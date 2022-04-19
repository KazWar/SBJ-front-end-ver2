using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingLineProducts.Dtos;
using RMS.Dto;


namespace RMS.SBJ.HandlingLineProducts
{
    public interface IHandlingLineProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHandlingLineProductForViewDto>> GetAll(GetAllHandlingLineProductsInput input);

		Task<GetHandlingLineProductForEditOutput> GetHandlingLineProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHandlingLineProductDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<HandlingLineProductHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<HandlingLineProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}