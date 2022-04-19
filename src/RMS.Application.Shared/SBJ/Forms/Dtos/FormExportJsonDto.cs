using System.Collections.Generic;
using System.Text;
using System;
using Abp.Application.Services.Dto;
using Newtonsoft.Json;

namespace RMS.SBJ.Forms.Dtos
{
	 
	public class FormExportJsonDto
	{
        public long FormLocaleId { get; set; }
        public long FormId { get; set; }       
        public string CompanyName { get; set; }
        public long CountryCode { get; set; }
        public string  LanguageCode { get; set; }
        public string Version { get; set; }
        public List<FormBlocksExportJsonDto> ExportBlocks { get; set; }



    }
}