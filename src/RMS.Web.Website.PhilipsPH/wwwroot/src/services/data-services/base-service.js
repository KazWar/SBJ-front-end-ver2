import axios from '@/plugins/axios';
export const useBaseService = () => {
    //* Get the options object and return it spread out for axios
    const getRequestOptions = (options) => {
        return {
            ...options
        };
    };
    //* Base axios request, supplement with options
    const request = async (options) => {
        const Options = getRequestOptions(options);
        try {
            const response = await axios(Options);
            if (response.status === 200) {
                return response;
            }
        }
        catch (error) {
            console.log(error);
        }
    };
    return {
        request
    };
};
//# sourceMappingURL=base-service.js.map