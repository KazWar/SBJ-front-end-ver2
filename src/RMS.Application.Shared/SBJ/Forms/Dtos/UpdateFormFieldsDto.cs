using System.Collections.Generic;

namespace RMS.SBJ.Forms.Dtos
{
    public class UpdateFormFieldsDto
    {
        public IEnumerable<GetFormBlockFieldForEditDto> EditedFormFields { get; set; }
    }
}
