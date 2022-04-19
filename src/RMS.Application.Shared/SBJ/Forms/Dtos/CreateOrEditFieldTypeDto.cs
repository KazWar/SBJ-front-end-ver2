
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class CreateOrEditFieldTypeDto : EntityDto<long?>
    {

		public string Description { get; set; }
		
		

    }
}