using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;

using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.PurchaseRegistrationFormFields
{
    public class CreateOrEditPurchaseRegistrationFormFieldModalViewModel
    {
       public CreateOrEditPurchaseRegistrationFormFieldDto PurchaseRegistrationFormField { get; set; }

	   		public string FormFieldDescription { get; set;}


       
	   public bool IsEditMode => PurchaseRegistrationFormField.Id.HasValue;
    }
}