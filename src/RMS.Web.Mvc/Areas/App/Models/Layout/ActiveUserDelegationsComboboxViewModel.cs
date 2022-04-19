using System.Collections.Generic;
using RMS.Authorization.Delegation;
using RMS.Authorization.Users.Delegation.Dto;

namespace RMS.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
