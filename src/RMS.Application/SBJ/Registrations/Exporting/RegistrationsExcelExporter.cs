using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Registrations.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Registrations.Exporting
{
    public class RegistrationsExcelExporter : NpoiExcelExporterBase, IRegistrationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RegistrationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRegistrationForViewDto> registrations)
        {
            return CreateExcelPackage(
                "Registrations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Registrations"));

                    AddHeader(
                        sheet,
                        L("FirstName"),
                        L("LastName"),
                        L("Street"),
                        L("HouseNr"),
                        L("PostalCode"),
                        L("City"),
                        L("EmailAddress"),
                        L("PhoneNumber"),
                        L("CompanyName"),
                        L("Gender"),
                        L("CountryId"),
                        L("CampaignId"),
                        (L("RegistrationStatus")) + L("StatusCode"),
                        (L("FormLocale")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, registrations,
                        _ => _.Registration.FirstName,
                        _ => _.Registration.LastName,
                        _ => _.Registration.Street,
                        _ => _.Registration.HouseNr,
                        _ => _.Registration.PostalCode,
                        _ => _.Registration.City,
                        _ => _.Registration.EmailAddress,
                        _ => _.Registration.PhoneNumber,
                        _ => _.Registration.CompanyName,
                        _ => _.Registration.Gender,
                        _ => _.Registration.CountryId,
                        _ => _.Registration.CampaignId,
                        _ => _.RegistrationStatusStatusCode,
                        _ => _.FormLocaleDescription
                        );

					
					
                });
        }
    }
}
