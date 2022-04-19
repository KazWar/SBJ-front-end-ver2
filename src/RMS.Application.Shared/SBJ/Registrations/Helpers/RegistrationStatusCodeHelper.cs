using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Registrations.Helpers
{
    public static class RegistrationStatusCodeHelper
    {
        public const string Pending = "100";
        public const string RequestedInvoiceCheck = "110";
        public const string Accepted = "200";
        public const string InProgress = "300";
        public const string Send = "400";
        public const string Incomplete = "500";
        public const string Rejected = "999";
    }
}