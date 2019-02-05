using System.Text.RegularExpressions;

namespace Sfa.Tl.Matching.Application
{
    public static class ValidationConstants
    {
        public static Regex UkprnRegex => 
            new Regex(@"^((?!(0))[0-9]{8})$");

        public static Regex PhoneNumberRegex => 
            new Regex(@"^[0-9+\s-()]{8,16}$");
    }
}