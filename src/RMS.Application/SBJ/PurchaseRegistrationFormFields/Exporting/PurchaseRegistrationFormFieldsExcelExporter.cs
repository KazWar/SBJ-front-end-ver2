using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Exporting
{
    public class PurchaseRegistrationFormFieldsExcelExporter : NpoiExcelExporterBase, IPurchaseRegistrationFormFieldsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PurchaseRegistrationFormFieldsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPurchaseRegistrationFormFieldForViewDto> purchaseRegistrationFormFields)
        {
            return CreateExcelPackage(
                "PurchaseRegistrationFormFields.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PurchaseRegistrationFormFields"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        (L("FormField")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, purchaseRegistrationFormFields,
                        _ => _.PurchaseRegistrationFormField.Description,
                        _ => _.FormFieldDescription
                        );

					
					
                });
        }
    }
}
