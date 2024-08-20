using System;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeHubPortal.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [Required]
        [MaxLength(1)]
        public string Role { get; set; } // "A" for Admin, "U" for User

        public DateTime CreatedAt { get; set; }
    }
}