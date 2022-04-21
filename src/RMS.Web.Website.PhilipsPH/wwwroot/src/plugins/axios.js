import axios from "axios";
export default axios.create({
    responseType: 'json',
    //* Default api headers, should be standard for all api calls
    headers: {
        "content-type": "application/json",
        "mode": "same-origin",
        "credentials": 'include',
        "redirect": 'follow'
    },
    //* Force parse the data part of the response
    //* Otherwise it remains a stringified text blob
    //* Axios already does this automatically once, but the API response is stringified twice.
    transformResponse: [
        ...axios.defaults.transformResponse,
        (data) => {
            data.content = JSON.parse(data.content);
            return data;
        }
    ]
});
//# sourceMappingURL=axios.js.map