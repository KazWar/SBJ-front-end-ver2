// https://ssl.ibanrechner.de/rest_validate_iban.html?&L=zcdrxbajeawl%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1%3FL%3D1

export class RechnerResponse {
    iban: string
    result: result
    return_code: return_codes
    bic_candidates: any

    constructor(data:any) {
        this.iban = data.iban
        this.result = data.result
        this.return_code = data.return_code
        this.bic_candidates = data.bic_candidates
    }
}

enum result {
    passed = "passed",
    failed = "failed"
}

enum return_codes {
    checks_passed = 0,
    subaccount_appended = 1,
    account_number_checksum_missing = 2,
    checksum_unchecked = 4,
    bankcode_unchecked = 8,
    subaccount_manual_verify = 32,
    account_number_checksum_error = 128,
    bankcode_not_found = 256,
    iban_len_incorrect = 512,
    bankcode_len_incorrect = 1024,
    iban_checksum_incorrect = 2048,
    input_data_missing = 4096,
    unsupported_country = 8192
}

