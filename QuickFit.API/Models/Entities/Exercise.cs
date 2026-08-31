using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("exercises")]
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int WorkoutSessionId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        
        public int DurationSeconds { get; set; }
        public int RestSeconds { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public int OrderIndex { get; set; }
        
        [MaxLength(255)]
        public string? VideoUrl { get; set; }
        
        [MaxLength(255)]
        public string? ImageUrl { get; set; }
        
        [Column(TypeName = "text")]
        public string? Tips { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("WorkoutSessionId")]
        public virtual WorkoutSession WorkoutSession { get; set; }
    }
}