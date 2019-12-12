using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerFeedbackDto
    {
        public int OpportunityItemId { get; set; }
        public Guid EmployerCrmId { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string JobRole { get; set; }
        public string JobRoleDetail =>
            string.IsNullOrEmpty(JobRole) || JobRole == "None given"
                ? $"{Route} {StudentsDetail}"
                : JobRole;
        public string Route { get; set; }
        public int? Placements { get; set; }
        public string PlacementsDetail =>
            Placements.HasValue
                ? Placements.ToString()
                : "At least 1";
        public string Town { get; set; }
        public string Postcode { get; set; }
        public DateTime? ModifiedOn { get; set; }
        private string StudentsDetail =>
            (Placements.HasValue && Placements.Value == 1)
            || PlacementsDetail == "At least 1"
                ? "student"
                : "students";
    }
}