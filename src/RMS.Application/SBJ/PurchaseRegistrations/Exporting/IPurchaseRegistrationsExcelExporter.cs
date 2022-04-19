using System.Collections.Generic;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using RMS.Dto;

namespace RMS.SBJ.PurchaseRegistrations.Exporting
{
    public interface IPurchaseRegistrationsExcelExporter
    {
        FileDto ExportToFile(List<GetPurchaseRegistrationForViewDto> purchaseRegistrations);
    }
}