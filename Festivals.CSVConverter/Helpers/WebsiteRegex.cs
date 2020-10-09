using System.Text.RegularExpressions;

namespace MoMi.CSVConverter.Helpers
{
    public static class WebsiteRegex
    {
        private const string WebsitePattern = @"^(https?:\/\/)?([\w\d-_]+)\.([\w\d-_\.]+)\/?\??([^#\n\r]*)?#?([^\n\r]*)";

        /// <summary>
        ///Checks urls to see if they match "website" urls.
        ///This excludes things like localhost, ftp, mailto and tel
        ///URL: regexr.com/3asir
        /// </summary>
        /// <param name="websiteString"></param>
        /// <returns>First website URL found in string</returns>
        public static string MatchWebsite(string websiteString)
        {
            if (!string.IsNullOrEmpty(websiteString))
            {
                Regex websiteRegex = new Regex(WebsitePattern);
                Match matchedWebsite = websiteRegex.Match(websiteString);

                if (matchedWebsite.Success)
                    return matchedWebsite.Value;
                return null;
            }

            return null;
        }
    }
}