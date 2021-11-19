using System.Text.RegularExpressions;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    // ReSharper disable UnusedMember.Global
    public static class ValidationConstants
    {
        public static Regex UkprnRegex => new(@"^((?!(0))[0-9]{8})$");

        public static Regex LarIdRegex => new(@"^[\da-zA-Z]{8}$");

        public static Regex UkPostcodeRegex => new(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})");
    }
}