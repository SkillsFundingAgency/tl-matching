namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmailsSentViewModel
    {
        public string EmployerContactName { get; set; }
        public string EmployerBusinessName { get; set; }

        private string _employerCrmId;
        public string EmployerCrmRecord
        {
            get => $"https://crm.employer.imservices.org.uk/EmployerCRM/main.aspx?etc=1&extraqs=formid%3d53e2f137-d7f8-4556-a260-bd320fa7e62c&id=%7b{_employerCrmId}%7d&pagetype=entityrecord";
            set => _employerCrmId = value;
        }
    }
}