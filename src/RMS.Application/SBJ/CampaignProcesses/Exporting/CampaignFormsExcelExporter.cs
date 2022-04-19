using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignFormsExcelExporter : NpoiExcelExporterBase, ICampaignFormsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignFormsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignFormForViewDto> campaignForms)
        {
            return CreateExcelPackage(
                "CampaignForms.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignForms"));

                    AddHeader(
                        sheet,
                        L("IsActive"),
                        (L("Campaign")) + L("Name"),
                        (L("Form")) + L("Version")
                        );

                    AddObjects(
                        sheet, 2, campaignForms,
                        _ => _.CampaignForm.IsActive,
                        _ => _.CampaignName,
                        _ => _.FormVersion
                        );

					
					
                });
        }
    }
}
