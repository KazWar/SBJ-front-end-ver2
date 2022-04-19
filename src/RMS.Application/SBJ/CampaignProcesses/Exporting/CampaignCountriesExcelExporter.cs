using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.CampaignProcesses.Exporting
{
    public class CampaignCountriesExcelExporter : NpoiExcelExporterBase, ICampaignCountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CampaignCountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCampaignCountryForViewDto> campaignCountries)
        {
            return CreateExcelPackage(
                "CampaignCountries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CampaignCountries"));

                    AddHeader(
                        sheet,
                        (L("Campaign")) + L("Name"),
                        (L("Country")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, campaignCountries,
                        _ => _.CampaignName,
                        _ => _.CountryDescription
                        );

                });
        }
    }
}