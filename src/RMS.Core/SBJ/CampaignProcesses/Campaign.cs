using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using RMS.SBJ.CampaignRetailerLocations;
using RMS.SBJ.ProductHandlings;

namespace RMS.SBJ.CampaignProcesses
{
    [Table("Campaign")]
    [Audited]
    public class Campaign : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual int? CampaignCode { get; set; }

        public virtual string ExternalCode { get; set; }

        public virtual string ExternalId { get; set; }

        public virtual string ThumbnailImagePath { get; set; }

        public virtual string BannerImagePath { get; set; }

        public virtual ICollection<CampaignCampaignType> CampaignCampaignTypes { get; set; }

        public virtual ICollection<CampaignForm> CampaignForms { get; set; }

        public virtual ICollection<CampaignMessage> CampaignMessages { get; set; }

        public virtual ICollection<CampaignRetailerLocation> CampaignRetailerLocations { get; set; }

        public virtual ICollection<ProductHandling> ProductHandlings { get; set; }
    }
}