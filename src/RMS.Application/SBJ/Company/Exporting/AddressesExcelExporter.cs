using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Company.Exporting
{
    public class AddressesExcelExporter : NpoiExcelExporterBase, IAddressesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AddressesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAddressForViewDto> addresses)
        {
            return CreateExcelPackage(
                "Addresses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Addresses"));

                    AddHeader(
                        sheet,
                        L("AddressLine1"),
                        L("AddressLine2"),
                        L("PostalCode"),
                        L("City"),
                        (L("Country")) + L("CountryCode")
                        );

                    AddObjects(
                        sheet, 2, addresses,
                        _ => _.Address.AddressLine1,
                        _ => _.Address.AddressLine2,
                        _ => _.Address.PostalCode,
                        _ => _.Address.City,
                        _ => _.CountryCountryCode
                        );

					
					
                });
        }
    }
}
