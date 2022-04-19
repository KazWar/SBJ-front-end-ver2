using RMS.Web.Areas.App.Models.CampaignTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.CampaignCampaignTypes
{
    public class CampaignCampaignTypeMultiSelectModel
    {
        public string CampaignTypeName { get; set; }

        public long? CampaignTypeId { get; set; }

        public long? CampaignId { get; set; }

        public long? CampaignCampaignTypeId { get; set; }

        public bool IsSelected { get; set; }
    }
}