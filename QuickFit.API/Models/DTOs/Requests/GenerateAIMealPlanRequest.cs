using System.Collections.Generic; 
namespace QuickFit.API.Models.DTOs.Requests 
{ 
    public class GenerateAIMealPlanRequest 
    { 
        public string Name { get; set; } 
        public string Age { get; set; } 
        public string BiologicalSex { get; set; } 
        public string Gender { get; set; } 
        public string Occupation { get; set; } 
        public string ActivityLevel { get; set; } 
        public string Height { get; set; } 
        public string Weight { get; set; } 
        public string Waist { get; set; } 
        public string Hip { get; set; } 
        public string MainGoal { get; set; } 
        public string GoalDetails { get; set; } 
        public string MedicalConditions { get; set; } 
        public string Allergies { get; set; } 
        public string Medications { get; set; } 
        public string SurgicalHistory { get; set; } 
        public List<string> DigestiveIssues { get; set; } 
        public string DietType { get; set; } 
        public string MealsPerDay { get; set; } 
        public List<string> HungerTimes { get; set; } 
        public string FirstMeal { get; set; } 
        public string LastMeal { get; set; } 
        public string DislikedFoods { get; set; } 
        public string FavoriteFoods { get; set; } 
        public string WaterIntake { get; set; } 
        public string BeverageConsumption { get; set; } 
        public string ExerciseType { get; set; } 
        public string ExerciseFrequency { get; set; } 
        public string ExerciseDuration { get; set; } 
        public string ExerciseTime { get; set; } 
        public string SleepHours { get; set; } 
        public string SleepQuality { get; set; } 
        public string StressLevel { get; set; } 
        public string WhoPreparesFood { get; set; } 
        public string EatsOutFrequency { get; set; } 
        public string Budget { get; set; } 
        public string CookingTime { get; set; } 
        public bool NeedPortable { get; set; } 
        public string PreferredSupport { get; set; } 
    }
}