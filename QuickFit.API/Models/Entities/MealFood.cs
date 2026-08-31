using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("meal_foods")]
    public class MealFood
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int MealId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FoodName { get; set; }
        
        [MaxLength(50)]
        public string Quantity { get; set; }
        
        public int Calories { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal Protein { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal Carbs { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal Fats { get; set; }
        
        public int OrderIndex { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("MealId")]
        public virtual Meal Meal { get; set; }
    }
}