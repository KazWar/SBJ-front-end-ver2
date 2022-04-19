using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Retailers.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Retailers.Exporting
{
    public class RetailersExcelExporter : NpoiExcelExporterBase, IRetailersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RetailersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRetailerForViewDto> retailers)
        {
            return CreateExcelPackage(
                "Retailers.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Retailers"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Code"),
                        (L("Country")) + L("CountryCode")
                        );

                    AddObjects(
                        sheet, 2, retailers,
                        _ => _.Retailer.Name,
                        _ => _.Retailer.Code,
                        _ => _.CountryCountryCode
                        );

					
					
                });
        }
    }
}
