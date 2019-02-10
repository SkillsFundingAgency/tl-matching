using System.Text.RegularExpressions;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    public static class ValidationConstants
    {
        public static Regex UkprnRegex => new Regex(@"^((?!(0))[0-9]{8})$");

        public static Regex LarsIdRegex => new Regex(@"^[\da-zA-Z]{8}$");
        
        public static Regex PhoneNumberRegex => new Regex(@"^[0-9+\s-()]{8,16}$");

        public static Regex UkPostCodeRegex => new Regex(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})");
    }
}