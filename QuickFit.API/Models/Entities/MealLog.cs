using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("meal_logs")]
    public class MealLog
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? MealId { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string MealType { get; set; }
        
        public bool Completed { get; set; }
        
        [Column(TypeName = "text")]
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("MealId")]
        public virtual Meal Meal { get; set; }
    }
}