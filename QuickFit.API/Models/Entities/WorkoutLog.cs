using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("workout_logs")]
    public class WorkoutLog
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? WorkoutSessionId { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        
        public int DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public bool Completed { get; set; }
        
        [Column(TypeName = "text")]
        public string Notes { get; set; }
        
        [Range(1, 5)]
        public int? Rating { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("WorkoutSessionId")]
        public virtual WorkoutSession WorkoutSession { get; set; }
    }
}