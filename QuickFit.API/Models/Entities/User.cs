using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "user"; // user, admin
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public virtual ICollection<MealPlan> MealPlans { get; set; }
        public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; }
        public virtual ICollection<MealLog> MealLogs { get; set; }
        public virtual ICollection<UserProgress> ProgressRecords { get; set; }
        public virtual ICollection<UserAchievement> Achievements { get; set; }
    }
}