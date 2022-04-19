using System.Collections.Generic;
using Abp.Application.Services.Dto;
using RMS.Configuration.Host.Dto;
using RMS.Editions.Dto;

namespace RMS.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}