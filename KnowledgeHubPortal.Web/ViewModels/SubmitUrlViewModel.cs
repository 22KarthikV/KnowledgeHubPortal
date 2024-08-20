
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KnowledgeHubPortal.Web.ViewModels
{
    public class SubmitUrlViewModel
    {
        [Required(ErrorMessage = "Link is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        //public List<SelectListItem> Categories { get; set; }
    }
}