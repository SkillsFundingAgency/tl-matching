using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public Guid LarsId { get; set; }
        public string QualificationTitle { get; set; }
        public string Summary { get; set; }
        public string Keywords { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<ProviderCourses> ProviderCourses { get; set; }
        public virtual ICollection<RoutePath> RoutePath { get; set; }
    }
}
