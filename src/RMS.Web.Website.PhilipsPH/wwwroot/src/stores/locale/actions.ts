import { useLocaleService } from "@/services"
import { useLocaleStore } from '@/stores'
import { storeToRefs } from "pinia"

export default {
    requireLocaleService() {
        return useLocaleService()
    },
    
    requireLocaleStore():any {
        return storeToRefs(useLocaleStore())
    },

    /**
     * Fetches the items directly from the API call,
     * without storing them in the state.
     * 
     * @returns An array of Locales or an Error if the function fails
     */
    async FetchLocales(): Promise<Locale[] | Error> {
        const { GetLocales } = this.requireLocaleService()

        return await GetLocales()
            .then(response => response)
            .catch(error => error)
    },

    /**
     * Returns the items if the state is saturated.
     * Otherwise it fetches them from the API, 
     * saturates the state and returns the items.
     * 
     * @returns An array of Locales or an Error if the function fails
     */
    async RequireLocales(): Promise<Locale[] | Error>{
        let { items } = $(this.requireLocaleStore())
        const { GetLocales } = this.requireLocaleService()

        //* Check if there are any items already in the state.
        if (items.length !== 0){
            return Promise.resolve(items)
        }

        return await GetLocales()
            .then(response => {
                items = response
                return response
            })
            .catch(error => error)
    }
}