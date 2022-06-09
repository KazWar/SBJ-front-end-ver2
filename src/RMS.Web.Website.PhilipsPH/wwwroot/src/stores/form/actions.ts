import { Form } from "@/common"
import { useFormService } from "@/services"
import { useFormStore } from '@/stores'
import { storeToRefs } from "pinia"

export default {
    requireFormService() {
        return useFormService()
    },
    
    requireFormStore():any {
        return storeToRefs(useFormStore())
    },

    /**
     * Fetches the form directly from the API call,
     * without storing it in the state.
     * 
     * @param id - Form id, refers to the specific database PK id of the form.
     * @returns A promise containing the form or an error.
     */
     async FetchForm(id:number):Promise<Form | Error> {
        if (id) return Error('Locale or id are empty or undefined')
        
        const { GetForm } = this.requireFormService()
        
        return await GetForm(id)
            .then(response => response)
            .catch(error => error)
    },

    /**
     * Returns the item if the state is saturated.
     * Otherwise it fetches it from the API, 
     * saturates the state and returns the item.
     * 
     * @param id - The form id of the selected campaign.
     * @returns A promise containing the form or an error.
     */
    async RequireForm(id:number):Promise<Form | Error> {
        let { item } = $(this.requireFormStore())
        const { GetForm } = this.requireFormService()

        //* Check if there is an item in the state already
        if (Object.keys(item).length != 0){
            return Promise.resolve(item)
        }

        return await GetForm(id)
            .then( response => {
                item = response
                return response
            })
            .catch(error => error)
    }
}