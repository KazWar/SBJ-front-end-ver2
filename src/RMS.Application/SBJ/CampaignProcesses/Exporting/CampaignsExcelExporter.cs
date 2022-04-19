using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignsExcelExporter : NpoiExcelExporterBase, ICampaignsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignForViewDto> campaigns)
        {
            return CreateExcelPackage(
                "Campaigns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Campaigns"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("StartDate"),
                        L("EndDate"),
                        L("CampaignCode"),
                        L("ExternalCode")
                        );

                    AddObjects(
                        sheet, 2, campaigns,
                        _ => _.Campaign.Name,
                        _ => _.Campaign.Description,
                        _ => _timeZoneConverter.Convert(_.Campaign.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Campaign.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Campaign.CampaignCode,
                        _ => _.Campaign.ExternalCode
                        );

					
					for (var i = 1; i <= campaigns.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);for (var i = 1; i <= campaigns.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}
