using RMS.SBJ.Forms.Dtos;
using System.Collections.Generic;
using Abp.Extensions;

namespace RMS.Web.Areas.App.Models.FormLocales
{
    public class CreateOrEditFormLocaleModalViewModel
    {
       public CreateOrEditFormLocaleDto FormLocale { get; set; }

	   		public string FormVersion { get; set;}

		public string LocaleDescription { get; set;}

        public List<FormLocaleLocaleLookupTableDto> FormLocaleLocaleList { get; set; }


        public bool IsEditMode => FormLocale.Id.HasValue;
    }
}