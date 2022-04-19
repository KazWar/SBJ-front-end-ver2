using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace RMS.SBJ.HandlingLineLogics
{
    [Table("HandlingLineLogic")]
    public class HandlingLineLogic : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal FirstHandlingLineId { get; set; }

        [Required]
        public virtual string Operator { get; set; }

        public virtual decimal SecondHandlingLineId { get; set; }
    }
}