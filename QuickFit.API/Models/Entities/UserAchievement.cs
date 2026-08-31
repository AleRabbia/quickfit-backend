using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("user_achievements")]
    public class UserAchievement
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Column(TypeName = "text")]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Icon { get; set; }
        
        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}