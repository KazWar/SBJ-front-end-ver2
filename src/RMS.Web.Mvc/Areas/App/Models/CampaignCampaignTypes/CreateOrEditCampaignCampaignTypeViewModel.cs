using RMS.SBJ.CampaignProcesses.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.CampaignCampaignTypes
{
    public class CreateOrEditCampaignCampaignTypeModalViewModel
    {
       public CreateOrEditCampaignCampaignTypeDto CampaignCampaignType { get; set; }

	   		public string CampaignDescription { get; set;}

		public string CampaignTypeName { get; set;}


       public List<CampaignCampaignTypeCampaignLookupTableDto> CampaignCampaignTypeCampaignList { get; set;}

public List<CampaignCampaignTypeCampaignTypeLookupTableDto> CampaignCampaignTypeCampaignTypeList { get; set;}


	   public bool IsEditMode => CampaignCampaignType.Id.HasValue;
    }
}