using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFit.API.Models.DTOs.Requests
{
    public class LogWorkoutRequest
    {
        public int? WorkoutSessionId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Range(1, 180)]
        public int DurationMinutes { get; set; }
        
        [Required]
        [Range(0, 2000)]
        public int CaloriesBurned { get; set; }
        
        [Required]
        public bool Completed { get; set; }
        
        public string Notes { get; set; }
        
        [Range(1, 5)]
        public int? Rating { get; set; }
    }
}