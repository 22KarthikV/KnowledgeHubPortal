// KnowledgeHubPortal.Web/ViewModels/HomeViewModel.cs
using System;
using System.Collections.Generic;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class HomeViewModel
    {
        public string TopContributor { get; set; }
        public string StarContributor { get; set; }
        public string MostPopularCategory { get; set; }
        public int TotalApprovedUrls { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public List<UrlViewModel> RecentUrls { get; set; }
    }
}