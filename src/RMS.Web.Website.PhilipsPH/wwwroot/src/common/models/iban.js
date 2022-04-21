// https://ssl.ibanrechner.de/rest_validate_iban.html?&L=zcdrxbajeawl%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1
export class RechnerResponse {
    iban;
    result;
    return_code;
    bic_candidates;
    constructor(data) {
        this.iban = data.iban;
        this.result = data.result;
        this.return_code = data.return_code;
        this.bic_candidates = data.bic_candidates;
    }
}
var result;
(function (result) {
    result["passed"] = "passed";
    result["failed"] = "failed";
})(result || (result = {}));
var return_codes;
(function (return_codes) {
    return_codes[return_codes["checks_passed"] = 0] = "checks_passed";
    return_codes[return_codes["subaccount_appended"] = 1] = "subaccount_appended";
    return_codes[return_codes["account_number_checksum_missing"] = 2] = "account_number_checksum_missing";
    return_codes[return_codes["checksum_unchecked"] = 4] = "checksum_unchecked";
    return_codes[return_codes["bankcode_unchecked"] = 8] = "bankcode_unchecked";
    return_codes[return_codes["subaccount_manual_verify"] = 32] = "subaccount_manual_verify";
    return_codes[return_codes["account_number_checksum_error"] = 128] = "account_number_checksum_error";
    return_codes[return_codes["bankcode_not_found"] = 256] = "bankcode_not_found";
    return_codes[return_codes["iban_len_incorrect"] = 512] = "iban_len_incorrect";
    return_codes[return_codes["bankcode_len_incorrect"] = 1024] = "bankcode_len_incorrect";
    return_codes[return_codes["iban_checksum_incorrect"] = 2048] = "iban_checksum_incorrect";
    return_codes[return_codes["input_data_missing"] = 4096] = "input_data_missing";
    return_codes[return_codes["unsupported_country"] = 8192] = "unsupported_country";
})(return_codes || (return_codes = {}));
//# sourceMappingURL=iban.js.map