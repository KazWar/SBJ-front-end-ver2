using RMS.SBJ.Company.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.ProjectManagers
{
    public class CreateOrEditProjectManagerModalViewModel
    {
       public CreateOrEditProjectManagerDto ProjectManager { get; set; }

	   		public string AddressPostalCode { get; set;}


       
	   public bool IsEditMode => ProjectManager.Id.HasValue;
    }
}