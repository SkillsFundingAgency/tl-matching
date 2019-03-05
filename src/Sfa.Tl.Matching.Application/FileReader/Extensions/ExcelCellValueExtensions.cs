using System;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    public static class ExcelCellValueExtensions
    {
        private const string Yes = "yes";
        private const string No = "no";

        public static int ToInt(this string cellValue)
        {
            return int.Parse(cellValue);
        }

        public static long ToLong(this string cellValue)
        {
            return long.Parse(cellValue);
        }

        public static Guid ToGuid(this string cellValue)
        {
            return Guid.Parse(cellValue);
        }

        public static bool ToBool(this string cellValue)
        {
            switch (cellValue.ToLower())
            {
                case Yes:
                    return true;
                case No:
                    return false;
            }

            throw new InvalidOperationException($"{nameof(cellValue)} cannot be parsed ({nameof(ToBool)})");
        }

        public static OfstedRating ToOfstedRating(this string cellValue)
        {
            var ofstedRating = cellValue.DehumanizeTo<OfstedRating>();

            return ofstedRating;
        }

        public static bool IsGuid(this string cellValue)
        {
            return Guid.TryParse(cellValue, out _);
        }

        public static bool IsYesNo(this string cellValue)
        {
            return cellValue == null || cellValue.ToLower() == Yes || cellValue.ToLower() == No;
        }

        public static bool IsOfstedRating(this string cellValue)
        {
            try
            {
                cellValue.DehumanizeTo<OfstedRating>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }
        
        public static bool IsAupaStatus(this string cellValue)
        {
            try
            {
                cellValue.DehumanizeTo<AupaStatus>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }
        
        public static bool IsCompanyType(this string cellValue)
        {
            if (string.IsNullOrWhiteSpace(cellValue)) return true;

            try
            {
                cellValue.DehumanizeTo<CompanyType>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
        }
    }
}