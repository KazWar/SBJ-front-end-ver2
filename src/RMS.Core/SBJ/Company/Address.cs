using RMS.SBJ.CodeTypeTables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using System.Collections.Generic;

namespace RMS.SBJ.Company
{
    [Table("Address")]
    [Audited]
    public class Address : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string AddressLine1 { get; set; }

        public virtual string AddressLine2 { get; set; }

        [Required]
        [StringLength(AddressConsts.MaxPostalCodeLength, MinimumLength = AddressConsts.MinPostalCodeLength)]
        public virtual string PostalCode { get; set; }

        [Required]
        public virtual string City { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<ProjectManager> ProjectManagers { get; set; }
    }
}