using System.Collections.Generic;

namespace RMS.SBJ.Registrations.Dtos.ProcessRegistration
{
    public class FormFieldCollectionLine
    {
        public long PurchaseRegId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set;}
        public IList<FormField> FormFields { get; set; }
    }
}
