
using System;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Core.Entities
{
    public class Statistic
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}