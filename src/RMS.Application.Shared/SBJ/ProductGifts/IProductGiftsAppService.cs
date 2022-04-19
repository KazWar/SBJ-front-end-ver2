using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.ProductGifts.Dtos;
using RMS.Dto;

namespace RMS.SBJ.ProductGifts
{
    public interface IProductGiftsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductGiftForViewDto>> GetAll(GetAllProductGiftsInput input);

        Task<GetProductGiftForViewDto> GetProductGiftForView(long id);

        Task<GetProductGiftForEditOutput> GetProductGiftForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductGiftDto input);

        Task Delete(EntityDto<long> input);

        Task<PagedResultDto<ProductGiftCampaignLookupTableDto>> GetAllCampaignForLookupTable(GetAllForLookupTableInput input);

    }
}