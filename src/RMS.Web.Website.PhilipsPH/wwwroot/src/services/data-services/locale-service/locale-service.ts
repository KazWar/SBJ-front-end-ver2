import { useBaseService } from '../base-service'
import { uniqueElements } from '@/common/utilities'
import { Locale } from '@/common/models'

//* Implement base service class
export const useLocaleService = () => {
    const { request } = useBaseService()

    const GetLocales = async () => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data }:any = await request({
                method:'get',
                url:'/api/v2/locale/'
            }
        )

        return processData(data.content)
    }

    //* Helper function to seperate extract & transform the locales from the combi-object
    const processData = (data:any) => {

        //* Extract the locale data from the campaign data
        const rawLocales = [...new Set(data.map((campaign:Campaign) => asLocale(campaign)))]

        //* Filter out the duplicate entries
        const locales = uniqueElements(rawLocales,'id')

        return { locales }
    }

    //* Helper function to transform json into a Locale class Object
    const asLocale = (data:any) =>{
        if (data) {
            return new Locale({
                id: data.localeId,
                name: data.localeName,
                description: data.localeDescription
            })
        }
    }

    return {
        GetLocales
    }
}
