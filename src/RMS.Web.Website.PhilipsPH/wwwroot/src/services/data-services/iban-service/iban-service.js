import { useBaseService } from '../base-service';
import { AssertNever, RechnerResponse } from '@/common';
/**
 * Returns a composer instance of the form service.
 */
export const useIbanService = () => {
    const { request } = useBaseService();
    const ValidateIban = async (iban) => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data } = await request({
            method: 'get',
            url: `/api/v2/iban/${iban}`
        });
        return processData(data.content);
    };
    /**
     * Parses JSON response data into a RechnerResponse type object
     *
     * @param response - Iban rechner response
     * @returns RechnerResponse type object
     */
    const processData = (response) => {
        return (response) ? new RechnerResponse(response) : AssertNever(response);
    };
    return {
        ValidateIban
    };
};
//# sourceMappingURL=iban-service.js.map