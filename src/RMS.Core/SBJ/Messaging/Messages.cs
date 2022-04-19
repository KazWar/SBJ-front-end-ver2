using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.SBJ.Messaging
{
    [Table("Messages")]
    public class Messages : Entity<int>
    {
        public virtual string Source { get; set; }

        public virtual string Reference { get; set; }

        public virtual string InitiatorReference { get; set; }
        public virtual string MessageInfo { get; set; }

        public virtual int MessageCollectionId { get; set; }

        public virtual int CurrentStepId { get; set; }
        public virtual int? TemplateId { get; set; }

        [StringLength(maximumLength: 2)]
        public virtual string LanguageCode { get; set; }

        [StringLength(maximumLength: 6)]
        public virtual string CountryCode { get; set; }
        public virtual string Subject { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string From { get; set; }
        public virtual string To { get; set; }
        public virtual string Bcc { get; set; }
        public virtual string Cc { get; set; }
        public virtual string Body { get; set; }
        public virtual string HeaderImage { get; set; }
        public virtual string HeaderText { get; set; }
        public virtual string GenericText1 { get; set; }
        public virtual string GenericText2 { get; set; }
        public virtual string GenericText3 { get; set; }
        public virtual string GenericText4 { get; set; }
        public virtual string GenericText5 { get; set; }

        [StringLength(maximumLength: 1)]
        public virtual string Gender { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Address4 { get; set; }
        public virtual string Address5 { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Phone { get; set; }
        public virtual DateTime? ExpirationTime { get; set; }
        public virtual string Attachement { get; set; }

        public virtual decimal Priority { get; set; }

        public virtual bool AwaitingSend { get; set; }

        public virtual bool SendError { get; set; }

        public virtual bool Finished { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual string PreviewFile { get; set; }
    }
}
