using System.Collections.Generic;
using RMS.Caching.Dto;

namespace RMS.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}