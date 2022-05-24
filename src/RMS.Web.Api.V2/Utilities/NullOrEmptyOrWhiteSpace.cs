namespace RMS.Web.Api.V2.Utilities
{
    public static class NullOrEmptyOrWhiteSpace
    {
        /// <summary>
        /// Checks whether the value is a null value, empty string or string containing only spaces.
        /// </summary>
        /// <param name="value">Any string value</param>
        /// <returns>FALSE if it's not empty, null or a space</returns>
        public static bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value) && value.Length == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
