using System;

namespace RMS.SBJ.Registrations.Dtos
{
    public sealed class GetRegistrationMessageHistoryEntryForViewOutput
    {
        public string MessageName { get; set; }

        public string Subject { get; set; }

        public string To { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }   
        
        public int StatusId { get; set; }
    }
}
