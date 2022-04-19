using System.Collections.Generic;
using Abp.Localization;
using RMS.Install.Dto;

namespace RMS.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
