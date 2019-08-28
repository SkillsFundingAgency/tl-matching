namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class MaintenanceViewModel
    {
        public bool IsOnline { get; set; }

        public string StatusDisplayText => IsOnline ? "Take offline" : "Bring online";
        public string BodyDisplayText => IsOnline ? "online" : "offline";
        public string ButtonClassName => IsOnline ? "tl-button--red govuk-button govuk-!-margin-right-1" : "govuk-button";
    }
}