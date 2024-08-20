using System;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Core.Entities
{
    public class Url
    {
        public int UrlId { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsApproved { get; set; }

        public DateTime SubmittedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}