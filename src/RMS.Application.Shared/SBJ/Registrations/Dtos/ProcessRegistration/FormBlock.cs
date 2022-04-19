using System.Collections.Generic;

namespace RMS.SBJ.Registrations.Dtos.ProcessRegistration
{
    /*public class FormBlock<T, U> where T : ProcessRegistrationFormField where U : FormFieldCollectionLine
    {
        public virtual string BlockTitle { get; set; }

       // public virtual IEnumerable<T> FormFields { get; set; }
        public IEnumerable<T> FormFields { get; set; }
    }

    public sealed class PurchaseRegistrationFormBlock
    {
        // public override IEnumerable<ProcessRegistrationFormField> FormFields { get => base.FormFields; set => base.FormFields = value; }
    }*/

    public sealed class FormBlock
    {
        public string BlockTitle { get; set; }
        public IList<FormField> FormFields { get; set; }
        public IList<FormFieldCollectionLine> FormFieldsCollectionLines { get; set; }
    }
}