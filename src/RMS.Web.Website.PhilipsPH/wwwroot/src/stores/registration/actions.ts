import { useFormService } from "@/services"
import { useRegistrationStore } from "@/stores"

export default {
    async postRegistration(locale:any, campaignCode:number): Promise<any> {
        const { PostForm } = useFormService()
        const { rawData } = useRegistrationStore()

        try {
            PostForm(locale, campaignCode, rawData)    
        } catch (error) {
            return error
        }
    }
}