import { Campaign } from '@/common/models'
import { useBaseService } from './base-service'

//* Implement base service class
export const useCampaignService = () => {
    const { request } = useBaseService()

    /**
     * Makes a GET call to the /Campaign API endpoint
     * 
     * @returns A RestSharp RestResponse object containing an array of Campaign type objects
     */
    const GetCampaigns = async (locale:string):Promise<Campaign[]> => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:'campaign'
            }
        )
        
        return data.content.map((campaign:Campaign) => new Campaign(campaign))
    }

    return {
        GetCampaigns
    }
}
