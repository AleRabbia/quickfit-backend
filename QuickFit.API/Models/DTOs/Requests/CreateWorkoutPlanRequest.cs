using System.ComponentModel.DataAnnotations;

namespace QuickFit.API.Models.DTOs.Requests
{
    public class CreateWorkoutPlanRequest
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public string Goal { get; set; }
        
        [Required]
        public string ExperienceLevel { get; set; }
        
        [Required]
        [Range(5, 120)]
        public int DurationMinutes { get; set; }
        
        [Required]
        [Range(1, 7)]
        public int DaysPerWeek { get; set; }
        
        [Required]
        public string TrainingStyle { get; set; }
        
        [Required]
        public string TrainingPlace { get; set; }
        
        public List<string> Equipment { get; set; }
        public string MedicalHistory { get; set; }
        public string DislikedExercises { get; set; }
        
        [Required]
        public string TrainingFocus { get; set; }
        
        public List<string> TrainingDays { get; set; }
    }
}