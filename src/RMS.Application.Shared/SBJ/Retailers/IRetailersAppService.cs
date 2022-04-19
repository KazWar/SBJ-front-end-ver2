using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.Dto;
using RMS.SBJ.Retailers.Dtos;

namespace RMS.SBJ.Retailers
{
    public interface IRetailersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRetailerForViewDto>> GetAll(GetAllRetailersInput input);

		Task<IEnumerable<GetRetailerForViewDto>> GetAllWithoutPaging();

		Task<PagedResultDto<GetRetailerForCampaignViewDto>> GetAllRetailersForCampaign(long campaignId);

		Task<GetRetailerForViewDto> GetRetailerForView(long id);

		Task<GetRetailerForEditOutput> GetRetailerForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRetailerDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRetailersToExcel(GetAllRetailersForExcelInput input);

		Task<PagedResultDto<RetailerCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);
	}
}