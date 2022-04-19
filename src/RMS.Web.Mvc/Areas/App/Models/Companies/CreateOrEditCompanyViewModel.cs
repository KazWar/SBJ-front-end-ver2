using RMS.SBJ.Company.Dtos;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.Companies
{
    public class CreateOrEditCompanyModalViewModel
    {
       public CreateOrEditCompanyDto Company { get; set; }

	   		public string AddressPostalCode { get; set;}


	   public bool IsEditMode => Company.Id.HasValue;
    }
}