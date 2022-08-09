using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class ValueParserExtensions
    {
        /// <summary>
        /// The TextInfo.ToTitleCase ignores ALL CAPS as special case because of that ToLowerInvariant is required
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var artsAndPreps = new List<string>
                { "a", "an", "and", "any", "at", "for", "from", "into", "of", "on",
                    "or", "some", "the", "to", };

            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());

            var tokens = result.Split(new[] { ' ', '\t', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            result = tokens[0];
            tokens.RemoveAt(0);

            result += tokens.Aggregate(string.Empty, (prev, input)
                => prev +
                   (artsAndPreps.Contains(input.ToLower()) // If True
                       ? " " + input.ToLower()              // Return the prep/art lowercase
                       : " " + input));                   // Otherwise return the valid word.

            result = Regex.Replace(result, @"(?!^Out)(Out\s+Of)", "out of");
            //Fix S after apostrophe, if it is before space or at end of string
            result = Regex.Replace(result, @"(['’])S(\s|$)", "$1s$2");

            return result;
        }

        public static string ToLetterOrDigit(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty :
               new string(Array.FindAll(value.ToCharArray(), char.IsLetterOrDigit));
        }

        public static string ToLetter(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty :
                new string(Array.FindAll(value.ToCharArray(), char.IsLetter));
        }

        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }

        public static long ToLong(this string value)
        {
            return long.Parse(value);
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }

        public static Guid ToGuid(this string value)
        {
            return Guid.Parse(value);
        }

        public static bool ToBool(this string value)
        {
            return value.ToLower() switch
            {
                "yes" => true,
                "true" => true,
                "no" => false,
                "false" => false,
                "-" => false,
                "_" => false,
                "" => false,
                _ => throw new ArgumentOutOfRangeException(nameof(value),
                    $"{nameof(value)} cannot be parsed ({nameof(ToBool)})")
            };
        }

        public static AupaStatus ToAupaStatus(this int value)
        {
            Enum.TryParse<AupaStatus>(value.ToString(), out var aupaStatus);
            return aupaStatus;
        }

        public static AupaStatus ToAupaStatus(this SfaAupa value)
        {
            return value.Value switch
            {
                229660000 => AupaStatus.Aware,
                229660001 => AupaStatus.Understand,
                229660002 => AupaStatus.Planning,
                229660003 => AupaStatus.Active,
                _ => throw new NotImplementedException()
            };
        }

        public static bool IsDateTime(this string value)
        {
            return DateTime.TryParse(value, out _);
        }

        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out _);
        }

        public static bool IsAupaStatus(this string value)
        {
            try
            {
                value.DehumanizeTo<AupaStatus>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }

        public static bool IsAupaStatus(this SfaAupa value)
        {
            try
            {
                //Aware	        229660000
                //Understand	229660001
                //Planning	    229660002
                //Active	    229660003
                if (value == null) return false;
                return (value.Value >= 0) && (value.Value == 229660000 || value.Value == 229660001 || value.Value == 229660002 || value.Value == 229660003);
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }

        public static bool IsCompanyType(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;

            try
            {
                value.DehumanizeTo<CompanyType>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }

        public static bool IsCompanyType(this Customertypecode value)
        {
            try
            {
                //Employer          200005
                //Employer Provider 200008
                if (value == null) return false;

                return (value.Value >= 0) && (value.Value == 200005 || value.Value == 200008);
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }

        public static string GetSearchTerm(this string[] searchTerms)
        {
            var searchTerm = new StringBuilder();
            foreach (var term in searchTerms)
                searchTerm.Append(term.ToQualificationSearch());

            return searchTerm.ToString();
        }

        public static string ToQualificationSearch(this string value)
        {
            if (value == null) return null;

            value = RemovePhrases(value);
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var words = value.Split(" ");
            var allowedWords = words.Except(QualificationTerms.Ignored, StringComparer.OrdinalIgnoreCase);
            var qualificationSearch = words.Where(x => allowedWords.Contains(x)).ToList();

            return string.Join(string.Empty, qualificationSearch).ToLetter();
        }

        public static bool IsAllSpecialCharactersOrNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;

            var countOfSpecialCharactersAndNumbers = 0;
            foreach (var c in value)
            {
                if (!char.IsLetter(c))
                    countOfSpecialCharactersAndNumbers++;
            }

            return value.Length == countOfSpecialCharactersAndNumbers;
        }

        private static string RemovePhrases(string value)
        {
            var termsWithMoreThanOneWord = QualificationTerms.Ignored
                .GroupBy(t => t.Split(" ").Length)
                .Where(i => i.Key > 1)
                .OrderByDescending(t => t.Key)
                .SelectMany(g => g);

            foreach (var t in termsWithMoreThanOneWord)
            {
                if (value.ToLower().Contains(t.ToLower()))
                    value = value.Replace(t, string.Empty, StringComparison.OrdinalIgnoreCase);

                if (string.IsNullOrEmpty(value))
                    return string.Empty;
            }

            return value;
        }
    }
}