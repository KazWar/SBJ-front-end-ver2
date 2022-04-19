using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RMS.SBJ.Forms.Dtos
{
    public class FormBlocksExportJsonDto
    {
        [JsonProperty("name")]
        public string Description { get; set; }

        public bool PurchaseRegistration { get; set; }

        public List<FormFieldsExportJsonDto> ExportFields { get; set; }

        

    }
}
