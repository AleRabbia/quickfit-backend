using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("workout_sessions")]
    public class WorkoutSession
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int WorkoutPlanId { get; set; }
        
        [MaxLength(20)]
        public string? DayOfWeek { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int EstimatedCalories { get; set; }
        
        [MaxLength(50)]
        public string? Difficulty { get; set; }
        
        public int OrderIndex { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan WorkoutPlan { get; set; }
        
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; }
    }
}