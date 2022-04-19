using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace RMS.SBJ.SystemTables.Dtos
{
    public class GetMessageForEditOutput
    {
		public CreateOrEditMessageDto Message { get; set; }

		public string SystemLevelDescription { get; set;}


    }
}