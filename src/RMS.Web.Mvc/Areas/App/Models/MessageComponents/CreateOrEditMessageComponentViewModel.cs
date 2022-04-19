using RMS.SBJ.CampaignProcesses.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MessageComponents
{
    public class CreateOrEditMessageComponentModalViewModel
    {
       public CreateOrEditMessageComponentDto MessageComponent { get; set; }

	   		public string MessageTypeName { get; set;}

		public string MessageComponentTypeName { get; set;}


       
	   public bool IsEditMode => MessageComponent.Id.HasValue;
    }
}