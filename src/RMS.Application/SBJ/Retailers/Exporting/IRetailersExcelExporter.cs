using System.Collections.Generic;
using RMS.SBJ.Retailers.Dtos;
using RMS.Dto;

namespace RMS.SBJ.Retailers.Exporting
{
    public interface IRetailersExcelExporter
    {
        FileDto ExportToFile(List<GetRetailerForViewDto> retailers);
    }
}