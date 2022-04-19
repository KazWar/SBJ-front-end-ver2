
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.PurchaseRegistrationFields.Dtos
{
    public class PurchaseRegistrationFieldDto : EntityDto<long>
    {
		public string Description { get; set; }


		 public long FormFieldId { get; set; }

		 
    }
}