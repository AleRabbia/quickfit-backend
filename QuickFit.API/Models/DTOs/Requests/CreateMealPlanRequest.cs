using System.ComponentModel.DataAnnotations;

namespace QuickFit.API.Models.DTOs.Requests
{
    public class CreateMealPlanRequest
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public string Goal { get; set; }
        
        [Required]
        public string DietType { get; set; }
        
        [Required]
        [Range(1000, 5000)]
        public int DailyCalories { get; set; }
        
        [Required]
        [Range(50, 300)]
        public int DailyProtein { get; set; }
        
        [Required]
        [Range(50, 500)]
        public int DailyCarbs { get; set; }
        
        [Required]
        [Range(20, 200)]
        public int DailyFats { get; set; }
        
        [Required]
        [Range(3, 6)]
        public int MealsPerDay { get; set; }
        
        public List<string> Allergies { get; set; }
        public List<string> Intolerances { get; set; }
        public List<string> DislikedFoods { get; set; }
        
        [Required]
        public string Budget { get; set; }
        
        [Required]
        [Range(10, 180)]
        public int CookingTime { get; set; }
    }
}