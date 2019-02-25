namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerSearchResultDto
    {
        public string EmployerName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string EmployerNameWithAka
        {
            get
            {
                var employerName = EmployerName;
                if (!string.IsNullOrEmpty(AlsoKnownAs))
                    employerName += $" ({AlsoKnownAs})";

                return employerName;
            }
        }
    }
}