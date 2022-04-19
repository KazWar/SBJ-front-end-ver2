using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.Forms.Dtos
{
    public class GetFormBlockForEditOutput
    {
		public CreateOrEditFormBlockDto FormBlock { get; set; }

		public string FormLocaleDescription { get; set;}


    }
}