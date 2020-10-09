using System.Text.RegularExpressions;

namespace RESTFestivals.CSVConverter.Helpers
{
    public static class TelephoneRegex
    {
        private const string TelephonePattern = @"(((\+[0-9]{1,2}|00[0-9]{1,2})[-\ .]?)?)(\d[-\ .]?){5,15}";

        /// <summary>
        ///Telephone number with national prefix
        ///URL: regexr.com/3asv7
        /// </summary>
        /// <param name="input"></param>
        /// <returns>First Telephone Number in string</returns>
        public static string MatchTelephone(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Regex telephoneRegex = new Regex(TelephonePattern);
                Match matchedTelephone = telephoneRegex.Match(input);

                if (matchedTelephone.Success)
                    return matchedTelephone.Value;
                return null;
            }

            return null;
        }
    }
}