using RMS.SBJ.HandlingLines.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.HandlingLines
{
    public class CreateOrEditHandlingLineViewModel
    {
        public CreateOrEditHandlingLineDto HandlingLine { get; set; }

        public string CampaignTypeName { get; set; }

        public string ProductHandlingDescription { get; set; }

        public List<HandlingLineCampaignTypeLookupTableDto> HandlingLineCampaignTypeList { get; set; }

        public bool IsEditMode => HandlingLine.Id.HasValue;
    }
}