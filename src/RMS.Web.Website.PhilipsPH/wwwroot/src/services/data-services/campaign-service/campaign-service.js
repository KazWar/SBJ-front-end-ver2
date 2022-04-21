import { useBaseService } from '../base-service';
import { Campaign } from '@/common/models';
//* Implement base service class
export const useCampaignService = () => {
    const { request } = useBaseService();
    const GetCampaigns = async (locale) => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data } = await request({
            method: 'get',
            url: `/api/v2/locale/${locale}/campaign`
        });
        return processData(data.content);
    };
    //* Helper function to seperate & transform the data into campaigns
    const processData = (data) => {
        //* Extract the campaign data from the response data
        const campaigns = [...new Set(data.map((campaign) => asCampaign(campaign)))];
        return { campaigns };
    };
    //* Helper function to transform the DTO into a Campaign class Object
    const asCampaign = (data) => {
        if (data) {
            return new Campaign({
                id: data.campaignId,
                code: data.campaignCode,
                name: data.campaignName,
                description: data.campaignDescription,
                thumbnail: data.campaignThumbnail,
                startDate: data.startDate,
                endDate: data.endDate,
                localeId: data.localeId,
                version: data.version
            });
        }
    };
    return {
        GetCampaigns
    };
};
//# sourceMappingURL=campaign-service.js.map