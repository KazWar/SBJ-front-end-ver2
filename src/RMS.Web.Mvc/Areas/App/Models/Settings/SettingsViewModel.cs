using System.Collections.Generic;
using Abp.Application.Services.Dto;
using RMS.Configuration.Tenants.Dto;

namespace RMS.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}