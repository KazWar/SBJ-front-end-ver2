using System.ComponentModel.DataAnnotations;

namespace RMS.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}