﻿namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class DashboardViewModel
    {
        public bool HasSavedOppportunities { get; set; }
        public bool IsServiceOnline { get; set; }
        public string HeaderText => IsServiceOnline ? "Take service offline" : "Put service back online";
        public string Description => IsServiceOnline ? string.Empty : "Under maintenance";
    }
}