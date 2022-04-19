using System.Collections.Generic;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using RMS.Dto;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Exporting
{
    public interface IPurchaseRegistrationFormFieldsExcelExporter
    {
        FileDto ExportToFile(List<GetPurchaseRegistrationFormFieldForViewDto> purchaseRegistrationFormFields);
    }
}