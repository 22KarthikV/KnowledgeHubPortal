using Microsoft.AspNetCore.Mvc.Rendering;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class SubmitUrlFormViewModel
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        
        public List<SelectListItem> Categories { get; set; }
    }   
}
