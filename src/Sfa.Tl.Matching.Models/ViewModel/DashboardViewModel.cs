namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class DashboardViewModel
    {
        public bool HasSavedOpportunities { get; set; }
        public bool IsServiceOnline { get; set; }
        public string HeaderText => IsServiceOnline ? "Take service offline" : "Put service back online";
        public string Description => IsServiceOnline ? "Show that the service is 'under maintenance'." : "Show that the service is no longer 'under maintenance'.";
    }
}