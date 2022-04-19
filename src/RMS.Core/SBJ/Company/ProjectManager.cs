using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace RMS.SBJ.Company
{
    [Table("ProjectManager")]
    [Audited]
    public class ProjectManager : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [StringLength(ProjectManagerConsts.MaxPhoneNumberLength, MinimumLength = ProjectManagerConsts.MinPhoneNumberLength)]
        public virtual string PhoneNumber { get; set; }

        [RegularExpression(ProjectManagerConsts.EmailAddressRegex)]
        public virtual string EmailAddress { get; set; }

        public virtual long AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address AddressFk { get; set; }
    }
}