using System;

namespace RMS.SBJ.Registrations.Dtos
{
    public sealed class GetRegistrationHistoryEntryForViewOutput
    {
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationStatusCode { get; set; }
        public string RegistrationStatusDescription { get; set; }
        public TypeOfChange TypeOfChange { get; set; }
    }

    public enum TypeOfChange
    {
        Status,
        Data
    }
}
