
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditValueListDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		
		public string ListValueApiCall { get; set; }
		
		

    }
}