using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LarsId { get; set; }
        public string QualificationTitle { get; set; }
        public string Summary { get; set; }
        public string Keywords { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<ProviderCourses> ProviderCourses { get; set; }
        public virtual ICollection<RoutePath> RoutePath { get; set; }
    }
}