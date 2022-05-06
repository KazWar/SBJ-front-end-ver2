// IBAN Rechner REST api documentation
// https://ssl.ibanrechner.de/rest_calculate_iban.html

namespace RMS.Web.Api.V2.Models
{
    public class IbanRechner
    {
        public char Iban { get; set; }
        public Results Result { get; set; }

        [CustomValidation(typeof(ReturnCodeValidationAttribute), "Validate")]
        public int ReturnCode { get; set; }

        
    }

    public class ReturnCodeValidationAttribute :ValidationAttribute
    {
        public int ReturnCode { get; set; }

        public ReturnCodeValidationAttribute(int returnCode)
        {
            ReturnCode = returnCode;
        }

        /// <summary>
        /// Validates 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            // Get the max possible value for return codes.
            var ReturnCodeMaxValue = Enum.GetValues(typeof(ReturnCodes)).Cast<byte>().Sum(x => x);

            // Get all enum values and reverse it.
            var ReturnCodesArray = Enum.GetValues(typeof(ReturnCodes)).Cast<int>().Reverse();

            // Check if the ReturnCode doesn't exceed maximum value.
            if (ReturnCode > ReturnCodeMaxValue)
            {
                return false;
            }

            // Check if you can substract the biggest enum value from the given value.
            foreach (int code in Enum.GetValues(typeof(ReturnCodes)))
            {
                if (ReturnCode % code != 0)
                {

                }
            }

                return base.IsValid(value);
        }
    }

    /// <summary>
    /// Enums which indicate whether the IBAN check passed (If all the checks succeeded) or failed (If any check failed).
    /// </summary>
    public enum Results : int
    {
        failed = 0,
        passed = 1
    }

    /// <summary>
    /// Return codes taken from the official API.
    /// </summary>
    [Flags]
    public enum ReturnCodes : int
    {
        [Description("All supported checks were done and passed.")]
        FULL_SUCCESS = 0,

        [Description("A subaccount number was automatically appended.")]
        SUB_ACC_APPEND = 1,

        [Description("Account number does not contain a checksum.")]
        ACC_NO_CHECKSUM = 2,

        [Description("Checksum was not checked.")]
        CHECKSUM_SKIPPED = 4,

        [Description("Bank code was not checked.")]
        BANKCODE_SKIPPED = 8,

        [Description("Warning: a subaccount number may need to be appended, but the necessity of this could not be automatically verified. Please verify manually.")]
        MANUAL_VERYIFY = 32,

        [Description("The bank has published an 'IBAN Rule' which states that for this account number/bank code combination, no IBAN should be generated.")]
        ACCOUNT_IBANRULE = 64,

        [Description("Checksum error in account number.")]
        CHECKSUM_ERROR = 128,

        [Description("Bankcode not found in directory.")]
        BANKCODE_NOT_FOUND = 256,

        [Description("IBAN has incorrect length.")]
        WRONG_IBAN_LENGTH = 512,

        [Description("Bank code has incorrect length.")]
        WRONG_BANKCODE_LENGTH = 1024,

        [Description("Missing input data (for example, the country code).")]
        MISSING_DATA = 4096,

        [Description("Country not yet supported.")]
        UNSUPPORTED_COUNTRY = 8192 
    }
}
