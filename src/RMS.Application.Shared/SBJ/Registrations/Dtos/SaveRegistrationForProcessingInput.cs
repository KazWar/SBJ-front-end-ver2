using RMS.SBJ.Registrations.Dtos.ProcessRegistration;
using System.Collections.Generic;

namespace RMS.SBJ.Registrations.Dtos
{
    public sealed class SaveRegistrationForProcessingInput
    {
        public long RegistrationId { get; set; }
        public IEnumerable<FormField> FormFields { get; set; }
        public long SelectedIncompleteReasonId { get; set; }
        public long SelectedRejectionReasonId { get; set; }
        public bool IsApproved { get; set; }
    }
}
