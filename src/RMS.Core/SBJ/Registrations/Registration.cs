using RMS.SBJ.CodeTypeTables;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using RMS.SBJ.CampaignProcesses;
using System.Collections.Generic;
using RMS.SBJ.ActivationCodeRegistrations;
using RMS.SBJ.PurchaseRegistrations;
using RMS.SBJ.RegistrationFormFieldDatas;

namespace RMS.SBJ.Registrations
{
    [Table("Registration")]
    public class Registration : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Gender { get; set; }

        public virtual string Street { get; set; }

        public virtual string HouseNr { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string City { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string CompanyName { get; set; }

        public virtual string Bic { get; set; }

        public virtual string Iban { get; set; }

        public virtual string IncompleteFields { get; set; }

        public virtual string RejectedFields { get; set; }     
        
        public virtual string Password { get; set; }

        public virtual long CampaignId { get; set; }

        public virtual long CampaignFormId { get; set; }

        public virtual long CountryId { get; set; }

        public virtual long LocaleId { get; set; }

        public virtual long RegistrationStatusId { get; set; }

        public virtual long? RejectionReasonId { get; set; }

        [ForeignKey("CampaignFormId")]
        public CampaignForm CampaignFormFk { get; set; }

        [ForeignKey("LocaleId")]
        public Locale LocaleFk { get; set; }

        [ForeignKey("RegistrationStatusId")]
        public RegistrationStatus RegistrationStatusFk { get; set; }

        [ForeignKey("RejectionReasonId")]
        public RejectionReason RejectionReasonFk { get; set; }

        public virtual ICollection<ActivationCodeRegistration> ActivationCodeRegistrations { get; set; }
        public virtual ICollection<PurchaseRegistration> PurchaseRegistrations { get; set; }
        public virtual ICollection<RegistrationFieldData> RegistrationFieldData { get; set; }
        public virtual ICollection<RegistrationHistory.RegistrationHistory> RegistrationHistories { get; set; }
    }
}