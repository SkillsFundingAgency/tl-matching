namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SentViewModel
    {
        public string EmployerContact { get; set; }
        //public int RouteId { get; set; }
        //public decimal? SearchRadius { get; set; }
        //public string Postcode { get; set; }
        //public string UserEmail { get; set; }
        //public string JobRole { get; set; }
        //public int? Placements { get; set; }

        private string _employerCrmId;
        public string EmployerCrmRecord
        {
            get => $"https://esfa-cs-prod.crm4.dynamics.com/main.aspx?pagetype=entityrecord&etc=1&id=%7b{_employerCrmId}%7d&extraqs=&newWindow=true";
            set => _employerCrmId = value;
        }
        // ProviderName
    }
}