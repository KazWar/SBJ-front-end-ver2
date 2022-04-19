using RMS.SBJ.SystemTables.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Messages
{
    public class CreateOrEditMessageModalViewModel
    {
       public CreateOrEditMessageDto Message { get; set; }

	   		public string SystemLevelDescription { get; set;}


       
	   public bool IsEditMode => Message.Id.HasValue;
    }
}