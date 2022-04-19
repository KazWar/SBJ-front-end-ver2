using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class MessageContentTranslationsExcelExporter : NpoiExcelExporterBase, IMessageContentTranslationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MessageContentTranslationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMessageContentTranslationForViewDto> messageContentTranslations)
        {
            return CreateExcelPackage(
                "MessageContentTranslations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("MessageContentTranslations"));

                    AddHeader(
                        sheet,
                        L("Content"),
                        (L("Locale")) + L("Description"),
                        (L("MessageComponentContent")) + L("Content")
                        );

                    AddObjects(
                        sheet, 2, messageContentTranslations,
                        _ => _.MessageContentTranslation.Content,
                        _ => _.LocaleDescription,
                        _ => _.MessageComponentContentContent
                        );

					
					
                });
        }
    }
}
