
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrationFormFields.Dtos
{
    public class PurchaseRegistrationFormFieldDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long? FormFieldId { get; set; }

		 
    }
}