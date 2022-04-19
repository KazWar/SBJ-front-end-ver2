using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CodeTypeTables.Exporting
{
    public class CampaignCategoriesExcelExporter : NpoiExcelExporterBase, ICampaignCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignCategoryForViewDto> campaignCategories)
        {
            return CreateExcelPackage(
                "CampaignCategories.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CampaignCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsActive"),
                        L("SortOrder")
                        );

                    AddObjects(
                        sheet, 2, campaignCategories,
                        _ => _.CampaignCategory.Name,
                        _ => _.CampaignCategory.IsActive,
                        _ => _.CampaignCategory.SortOrder
                        );

					
					
                });
        }
    }
}
