using RMS.SBJ.Forms.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.FormBlocks
{
    public class FormBlockViewModel : GetFormBlockForViewDto
    {
        public List<GetFormFieldForViewDto> UnSelectedFormBlockFields { get; set; }

        public List<GetFormBlockFieldForViewDto> SelectedFormBlockFields { get; set; }

    }
}