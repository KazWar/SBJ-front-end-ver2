using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormForEditOutput
    {
		public CreateOrEditFormDto Form { get; set; }

		public string SystemLevelDescription { get; set;}


    }
}