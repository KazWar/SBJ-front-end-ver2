using System.Collections.Generic;
using RMS.SBJ.Registrations.Dtos;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public sealed class RegistrationHistoryEntriesViewModel
    {
        public IEnumerable<GetRegistrationHistoryEntryForViewOutput> RegistrationHistoryEntries { get; set; }
    }
}
