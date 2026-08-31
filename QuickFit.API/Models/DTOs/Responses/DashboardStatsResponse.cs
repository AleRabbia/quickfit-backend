using System.Collections.Generic;

namespace QuickFit.API.Models.DTOs.Responses
{
    public class DashboardStatsResponse
    {
        public int WorkoutsCompleted { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public List<WeeklyActivityResponse> WeeklyActivity { get; set; }
        public List<AchievementResponse> RecentAchievements { get; set; }
    }
    
    public class WeeklyActivityResponse
    {
        public string Day { get; set; }
        public bool Completed { get; set; }
        public int Calories { get; set; }
    }
    
    public class AchievementResponse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string UnlockedAt { get; set; }
    }
}