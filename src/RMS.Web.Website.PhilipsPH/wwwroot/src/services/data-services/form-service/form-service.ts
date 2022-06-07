import { useBaseService } from '../base-service'
import { 
    AssertNever,
    Form
} from '@/common'

/**
 * Returns a composer instance of the form service.
 */
export const useFormService = () => {
    const { request } = useBaseService()

    const GetForm = async (locale:string, id:number): Promise<Form> => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:`form/${id}`,
                params:{
                    locale:locale
                }
            }
        )

        return processData(data.content)
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

        return processData(data.content)
    }

    return {
        GetForm,
        PostForm
    }
}