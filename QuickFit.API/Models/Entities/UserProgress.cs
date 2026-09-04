using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("user_progress")]
    public class UserProgress
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Waist { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Chest { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Hips { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Arms { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Legs { get; set; }
        
        [Column(TypeName = "decimal(4,2)")]
        public decimal? BodyFatPercentage { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? MuscleMass { get; set; }
        
        [Column(TypeName = "json")]
        public string? Photos { get; set; }
        
        [Column(TypeName = "text")]
        public string? Notes { get; set; }
        
        public int? WeeklyWorkouts { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}