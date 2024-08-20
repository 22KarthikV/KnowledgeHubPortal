
using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}