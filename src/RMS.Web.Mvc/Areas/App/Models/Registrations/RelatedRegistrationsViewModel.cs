using RMS.SBJ.Registrations.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Registrations
{
    public sealed class RelatedRegistrationsViewModel
    {
        public IEnumerable<GetRelatedRegistrationsForViewOutput> RelatedRegistrations { get; set; }
        public string TypeOfRelation { get; set; }
    }
}
