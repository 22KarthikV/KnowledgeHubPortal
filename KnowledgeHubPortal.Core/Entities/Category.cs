using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Core.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }


    }
}