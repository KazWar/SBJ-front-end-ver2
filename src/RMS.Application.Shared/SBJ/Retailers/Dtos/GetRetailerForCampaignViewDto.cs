using RMS.SBJ.RetailerLocations.Dtos;

namespace RMS.SBJ.Retailers.Dtos
{
    public sealed class GetRetailerForCampaignViewDto
    {
        public RetailerDto Retailer { get; set; }

        public RetailerLocationDto RetailerLocation { get; set; }
    }
}
