using System.Collections.Generic;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public interface IMessageContentTranslationsExcelExporter
    {
        FileDto ExportToFile(List<GetMessageContentTranslationForViewDto> messageContentTranslations);
    }
}