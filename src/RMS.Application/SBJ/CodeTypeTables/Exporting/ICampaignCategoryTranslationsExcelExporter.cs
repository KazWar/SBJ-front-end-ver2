using System.Collections.Generic;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public interface ICampaignCategoryTranslationsExcelExporter
    {
        FileDto ExportToFile(List<GetCampaignCategoryTranslationForViewDto> campaignCategoryTranslations);
    }
}