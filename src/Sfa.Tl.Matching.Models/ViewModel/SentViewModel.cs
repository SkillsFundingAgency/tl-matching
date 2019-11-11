namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SentViewModel
    {
        public string PrimaryContact { get; set; }
        public string CompanyName { get; set; }
        private string _employerCrmId;
        public string EmployerCrmRecord
        {
            get => $"https://esfa-cs-prod.crm4.dynamics.com/main.aspx?pagetype=entityrecord&etc=1&id=%7b{_employerCrmId}%7d&extraqs=&newWindow=true";
            set => _employerCrmId = value;
        }
    }
}