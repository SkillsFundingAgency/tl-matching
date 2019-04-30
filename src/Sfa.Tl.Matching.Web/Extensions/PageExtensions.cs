using System.Text;

namespace Sfa.Tl.Matching.Web.Extensions
{
    public class PageExtensions
    {
        private const string ServiceName = "Match employers with providers for industry placements";

        public static string GenerateTitle(bool isValid, string title)
        {
            var ignoreTitle = title == ServiceName;
            var titleSuffix = GetTitleSuffix();

            var formattedTitle = new StringBuilder();
            if (!isValid) formattedTitle.Append("Error: ");
            if (string.IsNullOrEmpty(title))
                return formattedTitle.Append(titleSuffix).ToString();

            if (!ignoreTitle) formattedTitle.Append($"{title} - ");
            formattedTitle.Append(titleSuffix);

            return formattedTitle.ToString();
        }

        private static string GetTitleSuffix()
        {
            const string govUk = "GOV.UK";

            return $"{ServiceName} - {govUk}";
        }
    }
}