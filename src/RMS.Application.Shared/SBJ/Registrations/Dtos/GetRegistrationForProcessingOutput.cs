using RMS.SBJ.CodeTypeTables.Dtos;
using RMS.SBJ.Registrations.Dtos.ProcessRegistration;
using System;
using System.Collections.Generic;

namespace RMS.SBJ.Registrations.Dtos
{
    public class GetRegistrationForProcessingOutput
    {
        public string CampaignTitle { get; set; }

        public CampaignType CampaignType { get; set; }

        public DateTime CampaignStartDate { get; set; }

        public DateTime CampaignEndDate { get; set; }

        public string DateCreated { get; set; }

        public IEnumerable<FormBlock> FormBlocks { get; set; }

        public CreateOrEditRegistrationDto Registration { get; set; }

        public IEnumerable<GetRelatedRegistrationsForViewOutput> RelatedRegistrationsByEmail { get; set; }

        public IEnumerable<GetRelatedRegistrationsForViewOutput> RelatedRegistrationsBySerialNumber { get; set; }

        public IEnumerable<GetRegistrationHistoryEntryForViewOutput> RegistrationHistoryEntries { get; set; }

        public IEnumerable<GetRegistrationMessageHistoryEntryForViewOutput> RegistrationMessageHistoryEntries { get; set; }

        public IEnumerable<GetRejectionReasonForViewDto> IncompleteReasons { get; set; }

        public IEnumerable<GetRejectionReasonForViewDto> RejectionReasons { get; set; }

        public long? SelectedIncompleteReasonId { get; set; }

        public long? SelectedRejectionReasonId { get; set; }

        public string PostalCountry { get; set; }

        public string StatusCode { get; set; }

        public bool StatusIsChangeable { get; set; }

        public TypeOfChange TypeOfChange { get; set; }
    }

    public enum CampaignType
    {
        Premium,
        CashRefund,
        ActivationCode,
        Other
    }
}
