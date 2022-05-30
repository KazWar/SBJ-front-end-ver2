import { useBaseService } from '../base-service'

//* Implement base service class
export const useLocaleService = () => {
    const { request } = useBaseService()

    /**
     * Makes a GET call to the /Locale API endpoint
     * 
     * @returns A RestSharp RestResponse object containing an array of Locale type objects
     */
    const GetLocales = async ():Promise<Locale[]> => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:'locale'
            }
        )

        return [...data.content]
    }

    return {
        GetLocales
    }
}
