using System;
using System.Globalization;
using System.Linq;
using Humanizer;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class ValueParserExtensions
    {
        private const string Yes = "yes";
        private const string No = "no";

        /// <summary>
        /// The TextInfo.ToTitleCase ignores ALL CAPS as special case because of that ToLowerInvariant is required
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty :
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
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
            switch (value.ToLower())
            {
                case Yes:
                    return true;
                case No:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} cannot be parsed ({nameof(ToBool)})");
            }
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