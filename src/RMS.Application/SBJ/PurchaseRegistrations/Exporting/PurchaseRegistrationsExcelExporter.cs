using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.PurchaseRegistrations.Exporting
{
    public class PurchaseRegistrationsExcelExporter : NpoiExcelExporterBase, IPurchaseRegistrationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PurchaseRegistrationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPurchaseRegistrationForViewDto> purchaseRegistrations)
        {
            return CreateExcelPackage(
                "PurchaseRegistrations.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PurchaseRegistrations"));

                    AddHeader(
                        sheet,
                        L("Quantity"),
                        L("TotalAmount"),
                        L("PurchaseDate"),
                        L("InvoiceImage"),
                        (L("Registration")) + L("FirstName"),
                        (L("Product")) + L("Ctn"),
                        (L("HandlingLine")) + L("CustomerCode"),
                        (L("RetailerLocation")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, purchaseRegistrations,
                        _ => _.PurchaseRegistration.Quantity,
                        _ => _.PurchaseRegistration.TotalAmount,
                        _ => _timeZoneConverter.Convert(_.PurchaseRegistration.PurchaseDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PurchaseRegistration.InvoiceImage,
                        _ => _.RegistrationFirstName,
                        _ => _.ProductCtn,
                        _ => _.HandlingLineCustomerCode,
                        _ => _.RetailerLocationName
                        );

					
					for (var i = 1; i <= purchaseRegistrations.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}
