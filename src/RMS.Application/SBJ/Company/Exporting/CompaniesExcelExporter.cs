using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Company.Exporting
{
    public class CompaniesExcelExporter : NpoiExcelExporterBase, ICompaniesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CompaniesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCompanyForViewDto> companies)
        {
            return CreateExcelPackage(
                "Companies.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Companies"));
                    

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("PhoneNumber"),
                        L("EmailAddress"),
                        L("BicCashBack"),
                        L("IbanCashBack"),
                        (L("Address")) + L("PostalCode")
                        );

                    AddObjects(
                        sheet, 2, companies,
                        _ => _.Company.Name,
                        _ => _.Company.PhoneNumber,
                        _ => _.Company.EmailAddress,
                        _ => _.Company.BICCashBack,
                        _ => _.Company.IBANCashBack,
                        _ => _.AddressPostalCode
                        );

					

                });
        }
    }
}
