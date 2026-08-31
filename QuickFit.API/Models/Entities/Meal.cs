using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("meals")]
    public class Meal
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int MealPlanId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string MealType { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "text")]
        public string Description { get; set; }
        
        [Column(TypeName = "time")]
        public TimeSpan? TimeOfDay { get; set; }
        
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }
        public int OrderIndex { get; set; }
        
        [MaxLength(255)]
        public string RecipeUrl { get; set; }
        
        [MaxLength(255)]
        public string ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("MealPlanId")]
        public virtual MealPlan MealPlan { get; set; }
        
        public virtual ICollection<MealFood> MealFoods { get; set; }
        public virtual ICollection<MealLog> MealLogs { get; set; }
    }
}