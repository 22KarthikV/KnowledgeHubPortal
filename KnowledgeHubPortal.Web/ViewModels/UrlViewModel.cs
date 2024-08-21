using System;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class UrlViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string CategoryName { get; set; }
        public string SubmitterName { get; set; }
        public DateTime ApprovalDate { get; set; }
    }
}