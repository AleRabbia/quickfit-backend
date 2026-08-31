using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickFit.API.Data;
using QuickFit.API.Models.DTOs.Responses;
using QuickFit.API.Services.Interfaces;

namespace QuickFit.API.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly QuickFitDbContext _context;

        public DashboardService(QuickFitDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsResponse> GetDashboardStats(int userId)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1); // Lunes

            // Total de entrenamientos completados este mes
            var workoutsCompleted = await _context.WorkoutLogs
                .Where(wl => wl.UserId == userId && wl.Date >= startOfMonth && wl.Completed)
                .CountAsync();

            // Racha actual
            var currentStreak = await CalculateCurrentStreak(userId);

            // Total de minutos este mes
            var totalMinutes = await _context.WorkoutLogs
                .Where(wl => wl.UserId == userId && wl.Date >= startOfMonth && wl.Completed)
                .SumAsync(wl => wl.DurationMinutes);

            // Calorías quemadas este mes
            var caloriesBurned = await _context.WorkoutLogs
                .Where(wl => wl.UserId == userId && wl.Date >= startOfMonth && wl.Completed)
                .SumAsync(wl => wl.CaloriesBurned);

            // Actividad semanal
            var weeklyActivity = await GetWeeklyActivity(userId, startOfWeek);

            // Logros recientes
            var recentAchievements = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.UnlockedAt)
                .Take(5)
                .Select(ua => new AchievementResponse
                {
                    Id = ua.Id,
                    Type = ua.Type,
                    Title = ua.Title,
                    Description = ua.Description,
                    Icon = ua.Icon,
                    UnlockedAt = ua.UnlockedAt.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return new DashboardStatsResponse
            {
                WorkoutsCompleted = workoutsCompleted,
                CurrentStreak = currentStreak,
                TotalMinutes = totalMinutes,
                CaloriesBurned = caloriesBurned,
                WeeklyActivity = weeklyActivity,
                RecentAchievements = recentAchievements
            };
        }

        private async Task<int> CalculateCurrentStreak(int userId)
        {
            var workoutLogs = await _context.WorkoutLogs
                .Where(wl => wl.UserId == userId && wl.Completed)
                .OrderByDescending(wl => wl.Date)
                .Select(wl => wl.Date)
                .Distinct()
                .ToListAsync();

            if (!workoutLogs.Any())
                return 0;

            int streak = 0;
            var currentDate = DateTime.Today;

            foreach (var logDate in workoutLogs)
            {
                if (logDate.Date == currentDate.Date)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else if (logDate.Date < currentDate.Date)
                {
                    break;
                }
            }

            return streak;
        }

        private async Task<List<WeeklyActivityResponse>> GetWeeklyActivity(int userId, DateTime startOfWeek)
        {
            var daysOfWeek = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            var weeklyActivity = new List<WeeklyActivityResponse>();

            for (int i = 0; i < 7; i++)
            {
                var date = startOfWeek.AddDays(i);
                var dayLogs = await _context.WorkoutLogs
                    .Where(wl => wl.UserId == userId && wl.Date.Date == date.Date && wl.Completed)
                    .ToListAsync();

                weeklyActivity.Add(new WeeklyActivityResponse
                {
                    Day = daysOfWeek[i],
                    Completed = dayLogs.Any(),
                    Calories = dayLogs.Sum(wl => wl.CaloriesBurned)
                });
            }

            return weeklyActivity;
        }
    }
}