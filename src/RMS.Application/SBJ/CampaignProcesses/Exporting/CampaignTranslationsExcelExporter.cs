using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignTranslationsExcelExporter : NpoiExcelExporterBase, ICampaignTranslationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignTranslationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignTranslationForViewDto> campaignTranslations)
        {
            return CreateExcelPackage(
                "CampaignTranslations.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CampaignTranslations"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        (L("Campaign")) + L("Name"),
                        (L("Locale")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, campaignTranslations,
                        _ => _.CampaignTranslation.Name,
                        _ => _.CampaignTranslation.Description,
                        _ => _.CampaignName,
                        _ => _.LocaleDescription
                        );

                });
        }
    }
}