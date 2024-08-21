// KnowledgeHubPortal.Web/ViewModels/SubmissionTrendsViewModel.cs
using System.Collections.Generic;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class SubmissionTrendsViewModel
    {
        public List<string> Dates { get; set; }
        public List<int> TotalSubmissions { get; set; }
        public List<int> ApprovedSubmissions { get; set; }
    }
}