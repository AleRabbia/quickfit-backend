using System;
using System.Collections.Generic;

namespace QuickFit.API.Models.DTOs.Responses
{
    public class MealPlanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public string DietType { get; set; }
        public int DailyCalories { get; set; }
        public int DailyProtein { get; set; }
        public int DailyCarbs { get; set; }
        public int DailyFats { get; set; }
        public int MealsPerDay { get; set; }
        public bool IsActive { get; set; }
        public bool GeneratedByAI { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MealResponse> Meals { get; set; }
    }
    
    public class MealResponse
    {
        public int Id { get; set; }
        public string MealType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TimeOfDay { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }
        public string RecipeUrl { get; set; }
        public string ImageUrl { get; set; }
        public List<MealFoodResponse> Foods { get; set; }
    }
    
    public class MealFoodResponse
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public string Quantity { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fats { get; set; }
    }
}