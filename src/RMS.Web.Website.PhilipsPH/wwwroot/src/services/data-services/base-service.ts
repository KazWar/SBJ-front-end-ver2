import axios from '@/plugins/axios'

export const useBaseService = () => {
    //* Get the options object and return it spread out for axios
    const getRequestOptions = (options?: {method?:any, url?:any, data?:any, params?:any, headers?:any}) => {
        return {
            ...options
        } 
    }

    //* Base axios request, supplement with options
    const request = async (options?: {method?:any, url?:any, data?:any, params?:any, headers?:any}) => {
        const Options = getRequestOptions(options)

        try {
            const response = await axios(Options)

            if (response.status === 200) {
                return response
            }
        } catch (error) {
            console.log(error)
        }
    }

    return {
        request
    }
}

