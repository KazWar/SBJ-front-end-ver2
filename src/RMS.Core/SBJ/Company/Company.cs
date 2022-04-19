using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.Company
{
    [Table("Company")]
    [Audited]
    public class Company : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxNameLength, MinimumLength = CompanyConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string PhoneNumber { get; set; }

        [RegularExpression(CompanyConsts.EmailAddressRegex)]
        public virtual string EmailAddress { get; set; }

        [RegularExpression(CompanyConsts.BicCashBackRegex)]
        public virtual string BicCashBack { get; set; }

        [RegularExpression(CompanyConsts.IbanCashBackRegex)]
        public virtual string IbanCashBack { get; set; }

        public virtual long AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address AddressFk { get; set; }
    }
}