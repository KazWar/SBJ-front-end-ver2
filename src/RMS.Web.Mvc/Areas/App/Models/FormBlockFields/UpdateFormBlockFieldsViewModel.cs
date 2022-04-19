using RMS.SBJ.Forms.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Web.Areas.App.Models.FormBlockFields
{
    public class UpdateFormBlockFieldsViewModel
    {
        public long FormBlockId { get; set; }

        public CreateOrEditFormBlockFieldDto[] SelectedFormFields { get; set; }

        public CreateOrEditFormBlockFieldDto[] AvailableFormFields { get; set; }
    }
}
