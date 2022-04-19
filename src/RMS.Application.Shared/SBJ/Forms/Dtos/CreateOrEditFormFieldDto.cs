
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
	public class CreateOrEditFormFieldDto : EntityDto<long?>
	{

		public string Description { get; set; }


		public string Label { get; set; }


		public string DefaultValue { get; set; }


		public int MaxLength { get; set; }


		public bool Required { get; set; }


		public bool ReadOnly { get; set; }


		public string InputMask { get; set; }


		public string RegularExpression { get; set; }


		public string ValidationApiCall { get; set; }


		public string RegistrationField { get; set; }
		
		
		public string PurchaseRegistrationField { get; set; }
		
		
		public bool IsPurchaseRegistration { get; set; }
		
		
		 public long FieldTypeId { get; set; }
		 
		 
    }
}