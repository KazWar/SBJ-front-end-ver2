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
     * Returns the item if the state is saturated.
     * Otherwise it fetches it from the API, 
     * saturates the state and returns the item.
     * 
     * @param {string} locale - Form locale eg: 'be_nl', 'be_fr', etc.
     * @param {number} id - Form id, refers to the specific database PK id of the campaign.
     * @returns {Promise<Form | Error>} A promise containing the form or an error.
     */
    async requireForm(locale:string, id:number):Promise<Form | Error> {
        if (locale || id) return Error('Locale or id are empty or undefined')
        
        let { item } = $(this.requireFormStore())
        const { GetForm } = this.requireFormService()

        //* Check if there is an item in the state already
        if (item.length !== 0){
            return Promise.resolve(item)
        }

        return await GetForm(locale, id)
            .then( response => {
                item = response
                return response
            })
            .catch(error => error)
    }
}