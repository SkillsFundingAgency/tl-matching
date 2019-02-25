using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerNameViewModel
    {
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "You must enter a business name")]
        public string BusinessName { get; set; }

        public string CompanyName
        {
            get
            {
                if (IsAkaIncluded())
                    return BusinessName.Substring(0, BusinessName.IndexOf(" (", StringComparison.Ordinal));

                return BusinessName;
            }
        }

        public string AlsoKnownAs
        {
            get
            {
                if (IsAkaIncluded())
                    return BusinessName.Split('(', ')')[1];

                return string.Empty;
            }
        }

        private bool IsAkaIncluded()
        {
            return BusinessName.Contains("(") && BusinessName.Substring(BusinessName.Length - 1) == ")";
        }
    }
}