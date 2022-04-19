using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignCampaignTypesExcelExporter : NpoiExcelExporterBase, ICampaignCampaignTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignCampaignTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignCampaignTypeForViewDto> campaignCampaignTypes)
        {
            return CreateExcelPackage(
                "CampaignCampaignTypes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignCampaignTypes"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        (L("Campaign")) + L("Description"),
                        (L("CampaignType")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, campaignCampaignTypes,
                        _ => _.CampaignCampaignType.Description,
                        _ => _.CampaignDescription,
                        _ => _.CampaignTypeName
                        );

					
					
                });
        }
    }
}
