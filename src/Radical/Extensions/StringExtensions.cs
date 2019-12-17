using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Radical
{
    /// <summary>
    /// Adds behaviors to the <c>string</c> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Performs a case insensitive like compare using the specified compare pattern.
        /// </summary>
        /// <param name="value">The source value to compare against the pattern.</param>
        /// <param name="pattern">The pattern to use as search pattern.</param>
        /// <returns><c>True</c> in case of successful match, otherwise <c>false</c>.</returns>
        /// <remarks>The default IsLike is performed using a case insensitive search.</remarks>
        public static bool IsLike(this string value, string pattern)
        {
            return IsLike(value, pattern, true);
        }

        /// <summary>
        /// Performs a case insensitive like compare using the specified compare patterns.
        /// </summary>
        /// <param name="value">The source value to compare against the pattern.</param>
        /// <param name="patterns">The patterns to use as search pattern.</param>
        /// <returns>
        ///     <c>True</c> in case of successful match, otherwise <c>false</c>.
        /// </returns>
        /// <remarks>The default IsLike is performed using a case insensitive search.</remarks>
        public static bool IsLike(this string value, params string[] patterns)
        {
            Ensure.That(patterns).Named("patterns")
                .IsNotNull();

            foreach (var pattern in patterns)
            {
                if (value.IsLike(pattern))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Performs a Like compare using the specified compare pattern.
        /// </summary>
        /// <param name="value">The source value to compare against the pattern.</param>
        /// <param name="pattern">The pattern to use a a search pattern.</param>
        /// <param name="ignoreCase"><c>True</c> to perform a case insensitive search, otherwise <c>false</c>.</param>
        /// <returns>
        ///     <c>True</c> in cas of successful match, otherwise <c>false</c>.
        /// </returns>
        public static bool IsLike(this string value, string pattern, bool ignoreCase)
        {
            if (value == null && pattern == null)
            {
                return true;
            }
            else if (value == null || pattern == null)
            {
                return false;
            }

            /*
             * Se nella stringa ci sono delle '\' dobbiamo 
             * metterci un bell'escape
             */
            pattern = pattern.Replace(@"\", @"\\");

            /*
             * Se nella stringa ci sono dei '.' dobbiamo 
             * metterci un bell'escape, questa operazione
             * è da fare dopo la precedente per evitare
             * di raddoppiare anche queste \
             */
            pattern = pattern.Replace(".", "\\.");

            /*
             * Gli '*' vengono sostituiti con '.*'
             */
            pattern = pattern.Replace("*", ".*");

            /*
             * I '?' vengono sostituiti con il semplice '.'
             */
            pattern = pattern.Replace("?", ".");

            /*
             * Includiamo il nostro pattern tra
             * \A e \z per fare in modo che matchi con
             * l'inizio e la fine della stringa altrimenti
             * ad es. Beatrice matcha con B*r 
             */
            pattern = string.Concat("\\A", pattern, "\\z");

            var options = RegexOptions.None;
            if (ignoreCase)
            {
                options |= RegexOptions.IgnoreCase;
            }

            return Regex.Match(value, pattern, options).Success;
        }

        /// <summary>
        /// Appends the specified text to the current string value.
        /// </summary>
        /// <param name="value">The current string value.</param>
        /// <param name="text">The text to append.</param>
        /// <returns>A new string composed by the source value with the given text appended at the end.</returns>
        public static string Append(this string value, string text)
        {
            return string.Concat(value, text);
        }

        /// <summary>
        /// If the input string is null returns the supplied default value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The input string, if not null, otherwise the supplied default value.</returns>
        public static string ValueOr(this string value, string defaultValue)
        {
            return ValueOr(value, defaultValue, null);
        }

        /// <summary>
        /// If the input string is null returns the supplied default value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="ifValue">A delegate that will be invoked, if the supplied value is not null, in 
        /// order to give the caller an opportunity to customize the return value; if this delegate is null 
        /// the incoming value is returned, otherwise is returned the return value of the supplied delegate.</param>
        /// <returns>
        /// The input string, if not null, otherwise the supplied default value.
        /// </returns>
        public static string ValueOr(this string value, string defaultValue, Func<string, string> ifValue)
        {
            if (value != null)
            {
                return ifValue != null ? ifValue(value) : value;
            }

            return defaultValue;
        }

        /// <summary>
        /// If the input string is null returns an empty string.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>The input string, if not null, otherwise an empty string.</returns>
        public static string ValueOrEmpty(this string value)
        {
            return ValueOr(value, string.Empty, null);
        }

        /// <summary>
        /// If the given string is null or empty returns the supplied default value.
        /// </summary>
        /// <param name="value">The value to test against.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The supplied value or default if the supplied one is null or empty.</returns>
        public static string IfNullOrEmptyReturn(this string value, string defaultValue)
        {
            var tmp = value.ValueOrEmpty().Trim();
            return string.IsNullOrEmpty(tmp) ? defaultValue : value;
        }

        /// <summary>
        /// If the input string is null returns an empty string.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="ifValue">A delegate that will be invoked, if the supplied value is not null, in 
        /// order to give the caller an opportunity to customize the return value; if this delegate is null 
        /// the incoming value is returned, otherwise is returned the return value of the supplied delegate.</param>
        /// <returns>
        /// The input string, if not null, otherwise an empty string.
        /// </returns>
        public static string ValueOrEmpty(this string value, Func<string, string> ifValue)
        {
            return ValueOr(value, string.Empty, ifValue);
        }

        /// <summary>
        /// Indicates whether the specified System.string object is null or an System.string.Empty string.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        ///     <c>true</c> if is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Splits the given source string using the supplied chars returning a list of
        /// distinct values.
        /// </summary>
        /// <param name="source">The source string to split.</param>
        /// <param name="separators">The char separators.</param>
        /// <returns>A distinct list of string split by the given chars.</returns>
        public static IEnumerable<string> AsKeywords(this string source, params Char[] separators)
        {
            return AsKeywords(source, true, separators);
        }

        /// <summary>
        /// Splits the given source string using the supplied chars returning a list of
        /// distinct values.
        /// </summary>
        /// <param name="source">The source string to split.</param>
        /// <param name="applyWildChardsIfNecessary">if set to <c>true</c> [apply wild chards if necessary].</param>
        /// <param name="separators">The char separators.</param>
        /// <returns>
        /// A distinct list of string split by the given chars.
        /// </returns>
        public static IEnumerable<string> AsKeywords(this string source, bool applyWildChardsIfNecessary, params Char[] separators)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new ReadOnlyCollection<string>(new List<string>());
            }

            return source.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(new List<string>(), (accumulator, word) =>
               {
                   var tmp = word.Trim();
                   if (tmp.Length > 0)
                   {
                       if (applyWildChardsIfNecessary)
                       {
                           tmp = ParseWildChars(tmp);
                       }

                       accumulator.Add(tmp);
                   }

                   return accumulator;
               })
                .Distinct();
        }

        static string ParseWildChars(string word)
        {
            if (word.IndexOfAny(new[] { '*', '?' }) == -1)
            {
                word = string.Concat("*", word, "*");
            }

            return word;
        }

        /// <summary>
        /// Returns the relative URI formatted as a pack URI: pack://.
        /// </summary>
        /// <param name="resourceRelativeUri">The resource relative URI.</param>
        /// <returns>The pack URI.</returns>
        public static string AsPackUri(this string resourceRelativeUri)
        {
            Ensure.That(resourceRelativeUri)
                .Named("resourceRelativeUri")
                .IsNotNullNorEmpty();

            var assembly = Assembly.GetCallingAssembly();
            string aName = assembly.GetName().Name;

            return resourceRelativeUri.AsPackUri(aName);
        }

        /// <summary>
        /// Returns the relative URI formatted as a pack URI: pack://.
        /// </summary>
        /// <param name="resourceRelativeUri">The resource relative URI.</param>
        /// <param name="assemblyName">Name of the assembly containing the resource.</param>
        /// <returns>The pack URI.</returns>
        public static string AsPackUri(this string resourceRelativeUri, string assemblyName)
        {
            Ensure.That(resourceRelativeUri).Named("resourceRelativeUri").IsNotNullNorEmpty();
            Ensure.That(assemblyName).Named("assemblyName").IsNotNullNorEmpty();

            var uri = string.Format("pack://application:,,,/{0};component{1}", assemblyName, resourceRelativeUri);
            return uri;
        }
    }
}
