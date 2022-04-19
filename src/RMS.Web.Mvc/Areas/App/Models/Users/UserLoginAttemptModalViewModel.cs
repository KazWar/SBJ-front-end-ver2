using System.Collections.Generic;
using RMS.Authorization.Users.Dto;

namespace RMS.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}