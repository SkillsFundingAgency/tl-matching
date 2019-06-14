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

            var words = value.Split(" ");
            for (var i = 0; i < words.Length; i++)
            {
                if (QualificationTerms.Ignored.Contains(words[i].ToLower()))
                    words[i] = "";
            }

            return string.Join(" ", words.Where(x => !string.IsNullOrEmpty(x))).ToLetter();
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
    }
}