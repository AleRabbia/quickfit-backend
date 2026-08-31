using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFit.API.Models.Entities
{
    [Table("meal_plans")]
    public class MealPlan
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Column(TypeName = "text")]
        public string Description { get; set; }
        
        [MaxLength(50)]
        public string Goal { get; set; }
        
        [MaxLength(50)]
        public string DietType { get; set; }
        
        public int DailyCalories { get; set; }
        public int DailyProtein { get; set; }
        public int DailyCarbs { get; set; }
        public int DailyFats { get; set; }
        public int MealsPerDay { get; set; }
        
        [Column(TypeName = "json")]
        public string Allergies { get; set; }
        
        [Column(TypeName = "json")]
        public string Intolerances { get; set; }
        
        [Column(TypeName = "json")]
        public string DislikedFoods { get; set; }
        
        [MaxLength(50)]
        public string Budget { get; set; }
        
        public int CookingTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool GeneratedByAI { get; set; } = false;
        
        [Column(TypeName = "text")]
        public string AIPrompt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public virtual ICollection<Meal> Meals { get; set; }
    }
}