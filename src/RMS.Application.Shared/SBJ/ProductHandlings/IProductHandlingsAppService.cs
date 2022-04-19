using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.ProductHandlings.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.ProductHandlings
{
    public interface IProductHandlingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductHandlingForViewDto>> GetAll(GetAllProductHandlingsInput input);

		Task<GetProductHandlingForEditOutput> GetProductHandlingForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductHandlingDto input);

		Task Delete(EntityDto<long> input);

		Task<PagedResultDto<ProductHandlingCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);

		//Task<GetProductHandlingForApiDto> GetProductHandlingForApi(long currentCampaignId);
		
	}
}