using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;
using static IdentityServer4.Models.IdentityResources;
using RMS.SBJ.Retailers;
using Address = RMS.SBJ.Company.Address;

namespace RMS.SBJ.CodeTypeTables
{
    [Table("Country")]
    [Audited]
    public class Country : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [RegularExpression(CountryConsts.CountryCodeRegex)]
        [StringLength(CountryConsts.MaxCountryCodeLength, MinimumLength = CountryConsts.MinCountryCodeLength)]
        public virtual string CountryCode { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Locale> Locales { get; set; }
        public virtual ICollection<Retailer> Retailers { get; set; }
    }
}