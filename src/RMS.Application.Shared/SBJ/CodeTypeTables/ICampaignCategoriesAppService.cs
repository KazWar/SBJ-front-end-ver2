using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CodeTypeTables
{
    public interface ICampaignCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignCategoryForViewDto>> GetAll(GetAllCampaignCategoriesInput input);

        Task<GetCampaignCategoryForViewDto> GetCampaignCategoryForView(long id);

		Task<GetCampaignCategoryForEditOutput> GetCampaignCategoryForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignCategoryDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignCategoriesToExcel(GetAllCampaignCategoriesForExcelInput input);

		
    }
}