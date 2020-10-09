using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RESTFestivals.CSVConverter.Helpers
{
    public static class RemoveCharacters
    {
        /// <summary>
        /// Removes all whitespaces from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A string without whitespaces</returns>
        public static string RemoveWhitespaces(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        /// <summary>
        /// Removes Special Characters from a string except
        /// Alphanumeric and passed in characters
        /// </summary>
        /// <returns>A string without special characters except the ones passed in </returns>
        public static string RemoveSpecialCharacters(string input, char[] exceptionChars)
        {
            string specialCharsPattern = @"[^A-z\s\d]";

            foreach (var exceptionChar in exceptionChars)
            {
                specialCharsPattern = specialCharsPattern.Insert(specialCharsPattern.IndexOf('d') + 1, exceptionChar.ToString());
            }

            input = Regex.Replace(input, specialCharsPattern, "");
            if (!string.IsNullOrEmpty(input))
                return input; 
            
            return null;
        }
        
        /// <summary>
        /// Removes all characters except numeric
        /// </summary>
        /// <returns>Number containing only numeric values</returns>
        public static T RemoveAllExceptNumeric<T>(string input)
        {
            int number;
            
            string numericPattern = @"[^0-9]";

            input = Regex.Replace(input, numericPattern, "");
            if (!string.IsNullOrEmpty(input))
            {
                if (Int32.TryParse(input, out number))
                {
                    return (T) Convert.ChangeType(number, typeof(T));
                }
            }

            return (T) Convert.ChangeType(null, typeof(T));
        }
    }
}