using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Products.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Products
{
    public interface IProductsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAllProductsForCampaign(long campaignId);

        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(long id);

		Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);
    }
}