import { useBaseService } from '../base-service';
import { AssertNever, Form } from '@/common';
/**
 * Returns a composer instance of the form service.
 */
export const useFormService = () => {
    const { request } = useBaseService();
    const GetForm = async (locale, id) => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data } = await request({
            method: 'get',
            url: `/api/v2/locale/${locale}/campaign/${id}/form`
        });
        return processData(data.content);
    };
    /**
     * Sends a POST request to the form api endpoint.
     *
     * @param locale - Locale code, E.G be_fr, be_nl, etc.
     * @param code - Campaign code
     * @returns HTTP response
     */
    const PostForm = async (locale, code, rawData) => {
        //! Extract the other contents of errors, success, etc... for later use
        const { data } = await request({
            method: 'post',
            url: `/api/v2/locale/${locale}/campaign/${code}/registration`,
            data: JSON.stringify({ ...rawData })
        });
        return processData(data.content);
    };
    /**
     * Parses JSON response data into a Form type object
     *
     * @param
     * @returns Form type object
     */
    const processData = (response) => {
        return (response) ? Form.fromDTO(response.formbuilder) : AssertNever(response);
    };
    return {
        GetForm,
        PostForm
    };
};
//# sourceMappingURL=form-service.js.map