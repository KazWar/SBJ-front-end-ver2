using System.Collections.Generic;
using RMS.SBJ.Registrations.Dtos;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public sealed class RegistrationMessageHistoryEntriesViewModel
    {
        public IEnumerable<GetRegistrationMessageHistoryEntryForViewOutput> RegistrationMessageHistoryEntries { get; set; }
    }
}
