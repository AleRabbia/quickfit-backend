using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("user_profiles")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? Age { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; }
        
        [MaxLength(20)]
        public string? Gender { get; set; }
        
        [MaxLength(50)]
        public string? ActivityLevel { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}