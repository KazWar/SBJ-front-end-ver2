using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;


namespace RMS.SBJ.CodeTypeTables
{
    public interface ICampaignCategoryTranslationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCampaignCategoryTranslationForViewDto>> GetAll(GetAllCampaignCategoryTranslationsInput input);

        Task<GetCampaignCategoryTranslationForViewDto> GetCampaignCategoryTranslationForView(long id);

		Task<GetCampaignCategoryTranslationForEditOutput> GetCampaignCategoryTranslationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCampaignCategoryTranslationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCampaignCategoryTranslationsToExcel(GetAllCampaignCategoryTranslationsForExcelInput input);

		
    }
}