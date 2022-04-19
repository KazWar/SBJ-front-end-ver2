import { useCampaignStore } from '@/stores'
import { useCampaignService } from "@/services"
import { storeToRefs } from 'pinia'

const requireCampaignService = () => {
    return useCampaignService()
}

const requireCampaignStore = () => {
    return storeToRefs(useCampaignStore())
}

export default {
    /**
     * Fetches the items directly from the API call,
     * without storing them in the state.
     * 
     * @returns 
     */
    async FetchCampaigns(locale:string): Promise<Campaign[] | unknown> {
        const { GetCampaigns } = requireCampaignService()

        //* Get the locales and return them directly.
        return await GetCampaigns(locale)
            .then(response => {
                return Promise.resolve(response.campaigns)
            })
            .catch(error => {
                return Promise.reject(error)
            })
    },

    /**
     * Returns the items if the state is saturated.
     * Otherwise it fetches them from the API, 
     * saturates the state and returns the items.
     * 
     * @returns 
     */
     async RequireCampaigns (locale:string): Promise<Campaign[] | unknown> {
        let { items } = $(requireCampaignStore())
        const { GetCampaigns } = requireCampaignService()

        //* Check if there are any items already in the state.
        if (items.length !== 0){
            return Promise.resolve(items)
        }

        //* Retrieve the campaigns from the API
        return await GetCampaigns(locale)
            .then(response => {
                items = response.campaigns
                return Promise.resolve(response.campaigns)
            })
            .catch(error => {
                return Promise.reject(error)
            })
    }
}