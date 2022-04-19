using System;
using System.Text;

namespace RMS.SBJ.Helpers
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(uint length, Random rand)
        {
            var characterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder((int)length);

            for(var i = 0; i < length; i++)
            {
                result.Append(characterSet[rand.Next(characterSet.Length)]);
            }

            return result.ToString();
        }
    }
}
