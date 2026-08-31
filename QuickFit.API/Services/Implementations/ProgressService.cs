using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickFit.API.Data;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.Entities;
using QuickFit.API.Services.Interfaces;

namespace QuickFit.API.Services.Implementations
{
    public class ProgressService : IProgressService
    {
        private readonly QuickFitDbContext _context;

        public ProgressService(QuickFitDbContext context)
        {
            _context = context;
        }

        public async Task<UserProgress> AddProgress(int userId, AddProgressRequest request)
        {
            var progress = new UserProgress
            {
                UserId = userId,
                Date = request.Date,
                Weight = request.Weight,
                Waist = request.Waist,
                Chest = request.Chest,
                Hips = request.Hips,
                Arms = request.Arms,
                Legs = request.Legs,
                BodyFatPercentage = request.BodyFatPercentage,
                MuscleMass = request.MuscleMass,
                Photos = request.Photos != null ? JsonSerializer.Serialize(request.Photos) : null,
                Notes = request.Notes,
                WeeklyWorkouts = request.WeeklyWorkouts,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserProgress.Add(progress);
            await _context.SaveChangesAsync();

            return progress;
        }

        public async Task<List<UserProgress>> GetUserProgress(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.UserProgress.Where(up => up.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(up => up.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(up => up.Date <= endDate.Value);
            }

            return await query.OrderByDescending(up => up.Date).ToListAsync();
        }

        public async Task<UserProgress> GetLatestProgress(int userId)
        {
            return await _context.UserProgress
                .Where(up => up.UserId == userId)
                .OrderByDescending(up => up.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteProgress(int progressId, int userId)
        {
            var progress = await _context.UserProgress
                .FirstOrDefaultAsync(up => up.Id == progressId && up.UserId == userId);

            if (progress == null)
            {return false;
            }

            _context.UserProgress.Remove(progress);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}