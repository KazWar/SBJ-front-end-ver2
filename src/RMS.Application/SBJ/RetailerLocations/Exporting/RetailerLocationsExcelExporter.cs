using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.RetailerLocations.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.RetailerLocations.Exporting
{
    public class RetailerLocationsExcelExporter : NpoiExcelExporterBase, IRetailerLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RetailerLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRetailerLocationForViewDto> retailerLocations)
        {
            return CreateExcelPackage(
                "RetailerLocations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("RetailerLocations"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("Retailer")) + L("Name"),
                        (L("Address")) + L("AddressLine1")
                        );

                    AddObjects(
                        sheet, 2, retailerLocations,
                        _ => _.RetailerLocation.Name,
                        _ => _.RetailerName,
                        _ => _.AddressAddressLine1
                        );

					
					
                });
        }
    }
}
