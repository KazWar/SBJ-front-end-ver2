using RMS.Sessions.Dto;

namespace RMS.Web.Views.Shared.Components.AccountLogo
{
    public class AccountLogoViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; }

        private string _skin = "light";
        private string _height = "50";

        public AccountLogoViewModel(GetCurrentLoginInformationsOutput loginInformations, string skin, string height)
        {
            LoginInformations = loginInformations;
            _skin = skin;
            _height = height;
        }

        public string GetLogoUrl(string appPath)
        {
            if (LoginInformations?.Tenant?.LogoId == null)
            {
                return appPath + "Common/Images/SBJ/SBJ-logo-wit.svg";
            }

            return appPath + "TenantCustomization/GetLogo?tenantId=" + LoginInformations?.Tenant?.Id;
        }

        public string GetLogoHeight()
        {
            return _height;
        }
    }
}