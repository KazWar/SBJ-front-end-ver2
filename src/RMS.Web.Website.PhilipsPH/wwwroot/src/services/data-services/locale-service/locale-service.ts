import { useBaseService } from '../base-service'

//* Implement base service class
export const useLocaleService = () => {
    const { request } = useBaseService()

    const GetLocales = async ():Promise<Locale[]> => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:'api/v2/locale'
            }
        )

        return [...data.content]
    }

    return {
        GetLocales
    }
}
