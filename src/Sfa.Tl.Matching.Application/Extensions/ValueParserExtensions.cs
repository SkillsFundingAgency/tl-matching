using System;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class ValueParserExtensions
    {
        private const string Yes = "yes";
        private const string No = "no";

        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }

        public static int? ToNullableInt(this string value)
        {
            return string.IsNullOrEmpty(value) ? default(int?) : int.Parse(value);
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
            }

            throw new InvalidOperationException($"{nameof(value)} cannot be parsed ({nameof(ToBool)})");
        }

        public static OfstedRating ToOfstedRating(this string value)
        {
            var ofstedRating = value.DehumanizeTo<OfstedRating>();

            return ofstedRating;
        }

        public static bool IsGuid(this string value)
        {
            return Guid.TryParse(value, out _);
        }

        public static bool IsYesNo(this string value)
        {
            return value == null || value.ToLower() == Yes || value.ToLower() == No;
        }

        public static bool IsOfstedRating(this string value)
        {
            try
            {
                value.DehumanizeTo<OfstedRating>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
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
    }
}