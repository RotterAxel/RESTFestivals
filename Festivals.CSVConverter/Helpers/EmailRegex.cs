using System.Text.RegularExpressions;

namespace MoMi.CSVConverter.Helpers
{
    public static class EmailRegex
    {
        private const string EmailPattern =
            @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        /// <summary>
        /// Email Validation as per RFC2822 standards.
        /// Straight from .net helpfiles :)
        /// URL: regexr.com/2rhq7
        /// </summary>
        /// <param name="emailString"></param>
        /// <returns>First email in string</returns>
        public static string MatchEmail(string emailString)
        {
            if (!string.IsNullOrEmpty(emailString))
            {
                Regex emailRegex = new Regex(EmailPattern);
                Match matchedEmail = emailRegex.Match(emailString);

                if (matchedEmail.Success)
                    return matchedEmail.Value;
                return null;
            }

            return null;
        }
    }
}