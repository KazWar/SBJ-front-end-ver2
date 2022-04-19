using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.Registrations.Dtos;
using RMS.SBJ.Registrations.Dtos.ProcessRegistration;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public sealed class ProcessingRegistrationViewModel
    {
        public int TenantId { get; set; }
        public string CampaignTitle { get; set; }

        public CampaignType CampaignType { get; set; }

        public string CampaignStartDate { get; set; }

        public string CampaignEndDate { get; set; }

        public string DateCreated { get; set; }

        public CreateOrEditRegistrationDto Registration { get; set; }

        public IEnumerable<GetRelatedRegistrationsForViewOutput> RelatedRegistrationsByEmail { get; set; }

        public IEnumerable<GetRelatedRegistrationsForViewOutput> RelatedRegistrationsBySerialNumber { get; set; }

        public IEnumerable<GetRegistrationHistoryEntryForViewOutput> RegistrationHistoryEntries { get; set; }

        public IEnumerable<GetRegistrationMessageHistoryEntryForViewOutput> RegistrationMessageHistoryEntries { get; set; }

        public IEnumerable<FormBlock> FormBlocks { get; set; }

        public bool IsEditMode => Registration.Id.HasValue;

        public IEnumerable<GetRejectionReasonForViewDto> IncompleteReasons { get; set; }

        public IEnumerable<GetRejectionReasonForViewDto> RejectionReasons { get; set; }

        public long? SelectedIncompleteReasonId { get; set; }

        public long? SelectedRejectionReasonId { get; set; }

        public string PostalCountry { get; set; }

        public string StatusCode { get; set; }

        public bool StatusIsChangeable { get; set; }

        public TypeOfChange TypeOfChange { get; set; }
    }
}
