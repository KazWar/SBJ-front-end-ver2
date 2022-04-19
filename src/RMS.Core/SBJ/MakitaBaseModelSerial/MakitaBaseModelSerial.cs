using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.MakitaBaseModelSerial
{
    [Table("MakitaBaseModelSerials")]
    public class MakitaBaseModelSerial : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string ItemNumber { get; set; }

        public virtual string BasisModel { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual long SerialNumberFrom { get; set; }

        public virtual long SerialNumberTo { get; set; }

    }
}