using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwEpApi.Extensions
{
    public static class StringExtensions
    {

        public static string ReplaceAt(this string input, int from, int to, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            StringBuilder builder = new StringBuilder(input);

            for(var i=from;i<=to;i++)
                builder[i] = newChar;

            return builder.ToString();
        }

        public static string MultipleReplace(this string text, Dictionary<string, string> replacements)
        {
            string retVal = text;
            foreach (string textToReplace in replacements.Keys)
            {
                retVal = retVal.Replace(textToReplace, replacements[textToReplace]);
            }
            return retVal;
        }

        public static string FirstCharToUpper(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0], new System.Globalization.CultureInfo("en-US") ) + s.Substring(1);
        }

    }
}
