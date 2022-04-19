using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class CampaignCategoryTranslationsExcelExporter : NpoiExcelExporterBase, ICampaignCategoryTranslationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignCategoryTranslationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignCategoryTranslationForViewDto> campaignCategoryTranslations)
        {
            return CreateExcelPackage(
                "CampaignCategoryTranslations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignCategoryTranslations"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("Locale")) + L("Description"),
                        (L("CampaignCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, campaignCategoryTranslations,
                        _ => _.CampaignCategoryTranslation.Name,
                        _ => _.LocaleDescription,
                        _ => _.CampaignCategoryName
                        );

					
					
                });
        }
    }
}
