import { useBaseService } from '../base-service'
import { Form } from '@/common'

/**
 * Returns a composer instance of the form service.
 */
export const useFormService = () => {
    const { request } = useBaseService()

    /**
     * Sends a POST request to the form api endpoint.
     * 
     * @param id - Form id, taken from selected campaign.
     * @param password - Optional password for form edits
     * @returns An empty form or a filled in form
     */
    const GetForm = async (id:number, password?:string): Promise<Form> => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:`form/${id}`,
                data:{
                    password:password
                }
            }
        )

        return data.content as Form
    }

    /**
     * Sends a POST request to the form api endpoint.
     * 
     * @param locale - Locale code, E.G be_fr, be_nl, etc.
     * @param code - Campaign code
     * @returns HTTP response
     */
    const PostForm = async(locale:string, code:number, rawData:any) => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'post',
                url:`/api/v2/locale/${locale}/campaign/${code}/registration`,
                data: JSON.stringify({...rawData})
            }
        )

        return data.content
    }

    return {
        GetForm,
        PostForm
    }
}