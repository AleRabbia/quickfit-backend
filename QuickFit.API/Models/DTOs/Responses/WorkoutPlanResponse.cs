using System;
using System.Collections.Generic;

namespace QuickFit.API.Models.DTOs.Responses
{
    public class WorkoutPlanResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public string ExperienceLevel { get; set; }
        public int DurationMinutes { get; set; }
        public int DaysPerWeek { get; set; }
        public string TrainingStyle { get; set; }
        public bool IsActive { get; set; }
        public bool GeneratedByAI { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<WorkoutSessionResponse> WorkoutSessions { get; set; }
    }
    
    public class WorkoutSessionResponse
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public string Name { get; set; }
        public int EstimatedCalories { get; set; }
        public string Difficulty { get; set; }
        public List<ExerciseResponse> Exercises { get; set; }
    }
    
    public class ExerciseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationSeconds { get; set; }
        public int RestSeconds { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Tips { get; set; }
    }
}