using System.Collections.Generic;
using RMS.SBJ.RetailerLocations.Dtos;
using RMS.Dto;

namespace RMS.SBJ.RetailerLocations.Exporting
{
    public interface IRetailerLocationsExcelExporter
    {
        FileDto ExportToFile(List<GetRetailerLocationForViewDto> retailerLocations);
    }
}