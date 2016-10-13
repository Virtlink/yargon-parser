using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofax.Terms.Extra
{
    // TODO: Move to utility library.

    public static class StringEscaper
    {
        /// <summary>
        /// Escapes the specified string.
        /// </summary>
        /// <param name="input">The string to escape.</param>
        /// <param name="sequences">A dictionary with for each character to escape, the escape sequence.</param>
        /// <returns>The escaped string.</returns>
        public static string Escape(string input, IReadOnlyDictionary<char, string> sequences)
        {
            #region Contract
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (sequences == null)
                throw new ArgumentNullException(nameof(sequences));
            #endregion

            StringBuilder sb = new StringBuilder();
            foreach (char ch in input)
            {
                string replacement;
                if (sequences.TryGetValue(ch, out replacement))
                    sb.Append(replacement);
                else
                    sb.Append(ch);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Unescapes the specified string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sequences"></param>
        /// <returns></returns>
        public static string Unescape(string input, IReadOnlyDictionary<string, char> sequences)
        {
            throw new NotImplementedException();
        }
    }
}
