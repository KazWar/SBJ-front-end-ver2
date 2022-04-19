using RMS.SBJ.CodeTypeTables.Dtos;
using System;

namespace RMS.SBJ.Registrations.Dtos
{
    public sealed class GetRelatedRegistrationsForViewOutput
    {
        public ulong RegistrationId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string SerialNumber { get; set; }
        public long StatusId{ get; set; }
        public string StatusDescription { get; set; }
        public string StatusCode { get; set; }

    }
}
