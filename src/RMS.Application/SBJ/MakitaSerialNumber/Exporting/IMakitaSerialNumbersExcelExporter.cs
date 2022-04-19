using System.Collections.Generic;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using RMS.Dto;

namespace RMS.SBJ.MakitaSerialNumber.Exporting
{
    public interface IMakitaSerialNumbersExcelExporter
    {
        FileDto ExportToFile(List<GetMakitaSerialNumberForViewDto> makitaSerialNumbers);
    }
}