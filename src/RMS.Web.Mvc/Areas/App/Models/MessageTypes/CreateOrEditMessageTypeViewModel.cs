using RMS.SBJ.CodeTypeTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.MessageTypes
{
    public class CreateOrEditMessageTypeModalViewModel
    {
       public CreateOrEditMessageTypeDto MessageType { get; set; }

	   		public string MessageVersion { get; set;}


       
	   public bool IsEditMode => MessageType.Id.HasValue;
    }
}