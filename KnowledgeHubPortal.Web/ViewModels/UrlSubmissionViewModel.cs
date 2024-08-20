using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class UrlSubmissionViewModel
    {
        [Required(ErrorMessage = "Link is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
