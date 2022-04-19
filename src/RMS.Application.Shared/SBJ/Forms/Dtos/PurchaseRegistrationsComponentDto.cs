using System.Collections.Generic;
using Newtonsoft.Json;


namespace RMS.SBJ.Forms.Dtos
{
    public sealed class PurchaseRegistrationsComponentDto
    {
        public List<GetFormFieldValueListDto> FormFieldValueList { get; set; }

        public List<DropdownListDto> DropdownList { get; set; }
    }
}
