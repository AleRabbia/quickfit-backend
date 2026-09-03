using System.ComponentModel.DataAnnotations;
using QuickFit.API.Validation;

namespace QuickFit.API.Models.DTOs.Requests
{
    public class CreateWorkoutPlanRequest
    {
        [Required]
        [MaxLength(WorkoutPlanConstraints.NameMaxLength)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        [MaxLength(WorkoutPlanConstraints.ShortTextMaxLength)]
        public string Goal { get; set; }
        
        [Required]
        [MaxLength(WorkoutPlanConstraints.ShortTextMaxLength)]
        public string ExperienceLevel { get; set; }
        
        [Required]
        [Range(WorkoutPlanConstraints.MinDurationMinutes, WorkoutPlanConstraints.MaxDurationMinutes)]
        public int DurationMinutes { get; set; }
        
        [Required]
        [Range(WorkoutPlanConstraints.MinDaysPerWeek, WorkoutPlanConstraints.MaxDaysPerWeek)]
        public int DaysPerWeek { get; set; }
        
        [Required]
        [MaxLength(WorkoutPlanConstraints.ShortTextMaxLength)]
        public string TrainingStyle { get; set; }
        
        [Required]
        [MaxLength(WorkoutPlanConstraints.ShortTextMaxLength)]
        public string TrainingPlace { get; set; }
        
        public List<string> Equipment { get; set; }
        public string MedicalHistory { get; set; }
        public string DislikedExercises { get; set; }
        
        [Required]
        [MaxLength(WorkoutPlanConstraints.ShortTextMaxLength)]
        public string TrainingFocus { get; set; }
        
        public List<string> TrainingDays { get; set; }
    }
}