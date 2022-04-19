
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormBlockDto : EntityDto<long>
    {
		public string Description { get; set; }

		public bool IsPurchaseRegistration { get; set; }

		public int SortOrder { get; set; }


		 public long FormLocaleId { get; set; }

		 
    }
}