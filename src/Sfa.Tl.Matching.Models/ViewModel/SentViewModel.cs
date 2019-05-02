namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SentViewModel
    {
        public string EmployerContact { get; set; }
        public int RouteId { get; set; }
        public decimal? SearchRadius { get; set; }
        public string Postcode { get; set; }
        public string UserEmail { get; set; }
        public string JobTitle { get; set; }
        public int? Placements { get; set; }
        public string EmployerName { get; set; }

        private string _employerCrmId;
        public string EmployerCrmRecord
        {
            get => $"https://crm.employer.imservices.org.uk/EmployerCRM/main.aspx?etc=1&extraqs=formid%3d53e2f137-d7f8-4556-a260-bd320fa7e62c&id=%7b{_employerCrmId}%7d&pagetype=entityrecord";
            set => _employerCrmId = value;
        }
        // ProviderName
    }
}