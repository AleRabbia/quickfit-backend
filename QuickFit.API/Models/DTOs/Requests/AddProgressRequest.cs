using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFit.API.Models.DTOs.Requests
{
    public class AddProgressRequest
    {
        [Required]
        public DateTime Date { get; set; }
        
        public decimal? Weight { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Hips { get; set; }
        public decimal? Arms { get; set; }
        public decimal? Legs { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? MuscleMass { get; set; }
        public List<string> Photos { get; set; }
        public string Notes { get; set; }
        public int? WeeklyWorkouts { get; set; }
    }
}