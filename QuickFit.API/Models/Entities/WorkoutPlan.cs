using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("workout_plans")]
    public class WorkoutPlan
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? Goal { get; set; }
        
        [MaxLength(50)]
        public string? ExperienceLevel { get; set; } 
        
        public int DurationMinutes { get; set; }
        public int DaysPerWeek { get; set; }
        
        [MaxLength(50)]
        public string? TrainingStyle { get; set; }
        
        [MaxLength(50)]
        public string? TrainingPlace { get; set; }
        
        [Column(TypeName = "json")]
        public string? Equipment { get; set; }
        
        [Column(TypeName = "text")]
        public string? MedicalHistory { get; set; }
        
        [Column(TypeName = "text")]
        public string? DislikedExercises { get; set; }
        
        [MaxLength(50)]
        public string? TrainingFocus { get; set; }
        
        public bool IsActive { get; set; } = true;
        public bool GeneratedByAI { get; set; } = false;
        
        [Column(TypeName = "text")]
        public string? AIPrompt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        
        public virtual ICollection<WorkoutSession>? WorkoutSessions { get; set; }
    }
}