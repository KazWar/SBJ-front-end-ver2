
using System;
using Abp.Application.Services.Dto;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormBlockFieldDto : EntityDto<long>
    {
		public int SortOrder { get; set; }


		 public long? FormFieldId { get; set; }

		 		 public long? FormBlockId { get; set; }

		 
    }
}