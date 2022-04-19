using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace RMS.SBJ.UniqueCodes
{
    [Table("UniqueCode")]
    public class UniqueCode : Entity<string>
    {

        public virtual bool Used { get; set; }

    }
}