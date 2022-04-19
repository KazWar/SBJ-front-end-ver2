using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using RMS.DataExporting.Excel.NPOI;
using RMS.SBJ.Forms.Dtos;
using RMS.Dto;
using RMS.Storage;

namespace RMS.SBJ.Forms.Exporting
{
    public class FormFieldsExcelExporter : NpoiExcelExporterBase, IFormFieldsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FormFieldsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFormFieldForViewDto> formFields)
        {
            return CreateExcelPackage(
                "FormFields.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("FormFields"));

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Label"),
                        L("DefaultValue"),
                        L("MaxLength"),
                        L("Required"),
                        L("ReadOnly"),
                        L("InputMask"),
                        L("RegularExpression"),
                        L("ValidationApiCall"),
                        L("RegistrationField"),
                        L("PurchaseRegistrationField"),
                        L("IsPurchaseRegistration"),
                        (L("FieldType")) + L("Description")
                        );

                    AddObjects(
                        sheet, 2, formFields,
                        _ => _.FormField.Description,
                        _ => _.FormField.Label,
                        _ => _.FormField.DefaultValue,
                        _ => _.FormField.MaxLength,
                        _ => _.FormField.Required,
                        _ => _.FormField.ReadOnly,
                        _ => _.FormField.InputMask,
                        _ => _.FormField.RegularExpression,
                        _ => _.FormField.ValidationApiCall,
                        _ => _.FormField.RegistrationField,
                        _ => _.FormField.PurchaseRegistrationField,
                        _ => _.FormField.IsPurchaseRegistration,
                        _ => _.FieldTypeDescription
                        );



                });
        }
    }
}
