using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameDevUtils.String
{
    public static class StringUtils
    {
        /// <param name="pascal">A pascal-case string</param>
        /// <returns>The given string with spaces between each two neighbor words.</returns>
        public static string SeparatePascal(this string pascal) =>
            Regex.Replace(pascal, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1]}");

        /// <summary>
        /// Split a text into multiple shorter fractions.
        /// </summary>
        /// <param name="text">The text to split</param>
        /// <param name="maxChars">The maximum allowed characters per fraction</param>
        /// <returns>A list of the text's fractions.</returns>
        public static List<string> SplitText(string text, int maxChars) {
            List<string> parts = new List<string>();

            for (int startIndex = 0; startIndex < text.Length; startIndex += maxChars) {
                int endIndex = Mathf.Min(startIndex + maxChars, text.Length);
                string fraction = text.Substring(startIndex, endIndex - startIndex);
                parts.Add(fraction);
            }

            return parts;
        }
    }
}
