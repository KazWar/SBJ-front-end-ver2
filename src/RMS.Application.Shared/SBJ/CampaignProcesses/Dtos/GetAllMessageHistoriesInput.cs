using Abp.Application.Services.Dto;
using System;

namespace RMS.SBJ.CampaignProcesses.Dtos
{
    public class GetAllMessageHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public long? MaxRegistrationIdFilter { get; set; }
        public long? MinRegistrationIdFilter { get; set; }

        public long? MaxAbpUserIdFilter { get; set; }
        public long? MinAbpUserIdFilter { get; set; }

        public string ContentFilter { get; set; }

        public DateTime? MaxTimeStampFilter { get; set; }
        public DateTime? MinTimeStampFilter { get; set; }

        public string MessageNameFilter { get; set; }

        public long? MaxMessageIdFilter { get; set; }
        public long? MinMessageIdFilter { get; set; }

        public string SubjectFilter { get; set; }

        public string ToFilter { get; set; }

    }
}