// KnowledgeHubPortal.Web/ViewModels/StatisticsViewModel.cs
using System;
using System.Collections.Generic;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class StatisticsViewModel
    {
        public string TopContributor { get; set; }
        public string StarContributor { get; set; }
        public string MostPopularCategory { get; set; }
        public int TotalApprovedUrls { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public List<RecentUrlViewModel> RecentApprovedUrls { get; set; }
        public SubmissionTrendsViewModel SubmissionTrends { get; set; }
        public List<CategoryViewModel> PopularCategories { get; set; }
        public List<ContributorViewModel> TopContributors { get; set; }
    }
}