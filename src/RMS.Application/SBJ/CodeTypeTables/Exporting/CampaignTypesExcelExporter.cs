using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class CampaignTypesExcelExporter : NpoiExcelExporterBase, ICampaignTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignTypeForViewDto> campaignTypes)
        {
            return CreateExcelPackage(
                "CampaignTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, campaignTypes,
                        _ => _.CampaignType.Name,
                        _ => _.CampaignType.IsActive
                        );

					
					
                });
        }
    }
}
