
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.PurchaseRegistrationFields.Dtos
{
    public class CreateOrEditPurchaseRegistrationFieldDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		 public long FormFieldId { get; set; }
		 
		 
    }
}