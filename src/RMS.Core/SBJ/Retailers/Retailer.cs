using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using RMS.SBJ.CodeTypeTables;
using System.Collections.Generic;
using RMS.SBJ.HandlingLineRetailers;
using RMS.SBJ.RetailerLocations;

namespace RMS.SBJ.Retailers
{
    [Table("Retailer")]
    public class Retailer : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual ICollection<HandlingLineRetailer> HandlingLineRetailers { get; set; }
        public virtual ICollection<RetailerLocation> RetailerLocations { get; set; }
    }
}