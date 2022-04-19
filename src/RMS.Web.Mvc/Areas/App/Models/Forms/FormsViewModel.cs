using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.Forms.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Forms
{
    public class FormsViewModel
    {
		public string FilterText { get; set; }

        public bool Editable { get; set; }

        public List<GetFormLocaleForViewDto> FormLocale { get; set; }

        public List<GetFormBlockForViewDto> MappedFormBlocks { get; set; }

        public List<GetFormBlockFieldForViewDto> MappedFormBlockFields { get; set; } 

        public List<GetFormBlockForViewDto> UnmappedFormBlocks { get; set; }

        public List<GetFormBlockFieldForViewDto> UnmappedFormBlockFields { get; set; }

        public List<GetFormBlockFieldForEditDto> UnmappedFormFields { get; set; }

        public List<GetLocaleForViewDto> Locale { get; set; }
    }   
}