using RMS.DataExporting.Excel.NPOI;
using RMS.Dto;
using RMS.SBJ.Report.GeneralReports.Dtos;
using RMS.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RMS.SBJ.Report.GeneralReports.Exporting
{
    public class GeneralReportExcelExporter : NpoiExcelExporterBase, IGeneralReportExcelExporter
    {

        public GeneralReportExcelExporter(
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        { }

        public FileDto ExportToFile(string name, List<GeneralReportDto> getGeneralReports)
        {
            var headers = new List<string>();
            var objects = new List<Func<GeneralReportDto, object>>();
            var generalReportWithRegistrationFields = getGeneralReports.Find(e => e.RegistrationFields != null);
            var generalReportWithPurchaseRegistrationFields = getGeneralReports.Find(e => e.RegistrationFields != null);
            var registrationFields = generalReportWithRegistrationFields != null ? generalReportWithRegistrationFields.RegistrationFields : new List<CustomField>();
            var purchaseRegistrationFields = generalReportWithPurchaseRegistrationFields != null ? generalReportWithPurchaseRegistrationFields.PurchaseRegistrationFields : new List<CustomField>();

            headers = generateHeaders(registrationFields, purchaseRegistrationFields);
            objects = generateObjects(registrationFields, purchaseRegistrationFields);

            var excelPackage = new FileDto();

            try
            {
                excelPackage = CreateExcelPackage(
                $"{name}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(name);

                    AddHeader(
                        sheet,
                        headers.ToArray()
                        );

                    AddObjects(
                        sheet, 2, getGeneralReports,
                        objects.ToArray()
                        );

                    for (var j = 0; j <= headers.Count; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                });
            }
            catch(Exception ex)
            {
                excelPackage.FileName = ex.Message;
            }

            return excelPackage;
        }

        List<string> generateHeaders(List<CustomField> registrationFields, List<CustomField> purchaseRegistrationFields)
        {
            var headers = new List<string>();
            headers.AddRange(new List<string>
            {
                "Campaign name",
                "Registration id",
                "Locale",
                "Registration time",
                "Company name",
                "Name",
                "Gender",
                "Street",
                "House number",
                "Postal code",
                "City",
                "Country",
                "Phone number",
                "Email",
                "Bic + Iban",
                "Current status",
                "Current status time",
                "Remarks"
            });

            if(registrationFields != null)
            {
                foreach(var customField in registrationFields)
                {
                    headers.Add(customField.Description);
                }
            }

            headers.AddRange(new List<string>
            {
                "Rejection reason",
                "Rejected fields",
                "Incomplete reason",
                "Incomplete fields",
                "Product purchased",
                "Quantity",
                "Purchase time",
                "Store purchased",
                "Address purchased",
                "Postal purchased",
                "City purchased",
                "Product selected",
                "Activationcode selected",
                "Cash refund"
            });

            if (purchaseRegistrationFields != null)
            {
                foreach (var customField in purchaseRegistrationFields)
                {
                    headers.Add(customField.Description);
                }
            }

            headers.AddRange(new List<string>
            {
                "Handling batch id",
                "Handling batch finished time"
            });

            return headers;
        }

        List<Func<GeneralReportDto, object>> generateObjects(List<CustomField> registrationFields, List<CustomField> purchaseRegistrationFields)
        {
            var objects = new List<Func<GeneralReportDto, object>>();

            objects.AddRange(new List<Func<GeneralReportDto, object>>
            {
                _ => _.CampaignName,
                _ => _.Id,
                _ => _.Locale,
                _ => _.RegistrationTime.Year != 1 ? _.RegistrationTime.ToString(new CultureInfo("nl-NL")) : null,
                _ => _.CompanyName,
                _ => _.Name,
                _ => _.Gender,
                _ => _.Street,
                _ => _.HouseNumber,
                _ => _.PostalCode,
                _ => _.City,
                _ => _.Country,
                _ => _.PhoneNumber,
                _ => _.Email,
                _ => _.BicIban,
                _ => _.CurrentStatus,
                _ => _.CurrentStatusTime.Year != 1 ? _.CurrentStatusTime.ToString(new CultureInfo("nl-NL")) : null,
                _ => _.Remarks
            });

            if(registrationFields != null)
            {
                for(var i = 0; i < registrationFields.Count; i++)
                {
                    var x = i;
                    objects.Add( ( _ => _.RegistrationFields.Count > 0 ? _.RegistrationFields[x].Value : null ) );
                }
            }

            objects.AddRange(new List<Func<GeneralReportDto, object>>
            {
                _ => _.RejectionReason,
                _ => _.RejectedFields,
                _ => _.IncompleteReason,
                _ => _.IncompleteFields,
                _ => _.ProductPurchased,
                _ => _.Quantity,
                _ => _.PurchaseTime.Year != 1 ? _.PurchaseTime.ToString(new CultureInfo("nl-NL")) : null,
                _ => _.StorePurchased,
                _ => _.AddressPurchased,
                _ => _.PostalPurchased,
                _ => _.CityPurchased,
                _ => _.ProductSelected,
                _ => _.ActivationcodeSelected,
                _ => _.CashRefund.HasValue ? _.CashRefund.Value.ToString("C", new CultureInfo("nl-NL")) : null
            });

            if (purchaseRegistrationFields != null)
            {
                for (var i = 0; i < purchaseRegistrationFields.Count; i++)
                {
                    var x = i;
                    objects.Add((_ => _.PurchaseRegistrationFields.Count > 0 ? _.PurchaseRegistrationFields[x].Value : null));
                }
            }

            objects.AddRange(new List<Func<GeneralReportDto, object>>
            {
                _ => _.HandlingBatchId,
                _ => _.HandlingBatchFinishedTime.Year != 1 ? _.HandlingBatchFinishedTime.ToString(new CultureInfo("nl-NL")) : null
            });

            return objects;
        }
    }
}
