using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuickFit.API.Data;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.DTOs.Responses;
using QuickFit.API.Models.Entities;
using QuickFit.API.Services.Interfaces;

namespace QuickFit.API.Services.Implementations
{
    public class WorkoutService : IWorkoutService
    {
        private readonly QuickFitDbContext _context;

        public WorkoutService(QuickFitDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutPlanResponse> CreateWorkoutPlan(int userId, CreateWorkoutPlanRequest request)
        {
            var workoutPlan = new WorkoutPlan
            {
                UserId = userId,
                Name = request.Name,
                Description = request.Description,
                Goal = request.Goal,
                ExperienceLevel = request.ExperienceLevel,
                DurationMinutes = request.DurationMinutes,
                DaysPerWeek = request.DaysPerWeek,
                TrainingStyle = request.TrainingStyle,
                TrainingPlace = request.TrainingPlace,
                Equipment = JsonSerializer.Serialize(request.Equipment ?? new List<string>()),
                MedicalHistory = request.MedicalHistory,
                DislikedExercises = request.DislikedExercises,
                TrainingFocus = request.TrainingFocus,
                IsActive = true,
                GeneratedByAI = false,
				AIPrompt = null, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(workoutPlan);
            await _context.SaveChangesAsync();

            // Crear sesiones de ejemplo (luego será generado por IA)
            await CreateSampleWorkoutSessions(workoutPlan.Id, request.TrainingDays);

            return await GetWorkoutPlanById(workoutPlan.Id, userId);
        }

        private async Task CreateSampleWorkoutSessions(int workoutPlanId, List<string> trainingDays)
        {
            if (trainingDays == null || !trainingDays.Any())
            {
                trainingDays = new List<string> { "Lunes", "Miércoles", "Viernes" };
            }

            var sessions = new List<WorkoutSession>();
            var orderIndex = 0;

            foreach (var day in trainingDays)
            {
                var session = new WorkoutSession
                {
                    WorkoutPlanId = workoutPlanId,
                    DayOfWeek = day,
                    Name = $"Entrenamiento {day}",
                    EstimatedCalories = 180,
                    Difficulty = "Intermedio",
                    OrderIndex = orderIndex++,
                    CreatedAt = DateTime.UtcNow
                };

                sessions.Add(session);
            }

            _context.WorkoutSessions.AddRange(sessions);
            await _context.SaveChangesAsync();

            // Agregar ejercicios de ejemplo a cada sesión
            foreach (var session in sessions)
            {
                await CreateSampleExercises(session.Id);
            }
        }

        // Reemplazar el método CreateSampleExercises en WorkoutService.cs

		private async Task CreateSampleExercises(int workoutSessionId)
		{
			var exercises = new List<Exercise>
			{
				new Exercise
				{
					WorkoutSessionId = workoutSessionId,
					Name = "Burpees",
					Description = "Ejercicio de cuerpo completo de alta intensidad",
					DurationSeconds = 45,
					RestSeconds = 15,
					OrderIndex = 0,
					Tips = "Mantén el core activado durante todo el movimiento",
					VideoUrl = null,
					ImageUrl = null,
					Sets = null,
					Reps = null,
					CreatedAt = DateTime.UtcNow
				},
				new Exercise
				{
					WorkoutSessionId = workoutSessionId,
					Name = "Mountain Climbers",
					Description = "Escaladores que trabajan core y cardio",
					DurationSeconds = 45,
					RestSeconds = 15,
					OrderIndex = 1,
					Tips = "Mantén las caderas bajas y el ritmo constante",
					VideoUrl = null,
					ImageUrl = null,
					Sets = null,
					Reps = null,
					CreatedAt = DateTime.UtcNow
				},
				new Exercise
				{
					WorkoutSessionId = workoutSessionId,
					Name = "Jump Squats",
					Description = "Sentadillas con salto para potencia de piernas",
					DurationSeconds = 45,
					RestSeconds = 15,
					OrderIndex = 2,
					Tips = "Aterriza suavemente y mantén buena forma",
					VideoUrl = null,
					ImageUrl = null,
					Sets = null,
					Reps = null,
					CreatedAt = DateTime.UtcNow
				},
				new Exercise
				{
					WorkoutSessionId = workoutSessionId,
					Name = "High Knees",
					Description = "Rodillas altas para cardio intenso",
					DurationSeconds = 45,
					RestSeconds = 15,
					OrderIndex = 3,
					Tips = "Eleva las rodillas al máximo posible",
					VideoUrl = null,
					ImageUrl = null,
					Sets = null,
					Reps = null,
					CreatedAt = DateTime.UtcNow
				},
				new Exercise
				{
					WorkoutSessionId = workoutSessionId,
					Name = "Plancha",
					Description = "Fortalecimiento de core",
					DurationSeconds = 45,
					RestSeconds = 15,
					OrderIndex = 4,
					Tips = "Mantén el cuerpo en línea recta, no subas ni bajes la cadera",
					VideoUrl = null,
					ImageUrl = null,
					Sets = null,
					Reps = null,
					CreatedAt = DateTime.UtcNow
				}
			};

			_context.Exercises.AddRange(exercises);
			await _context.SaveChangesAsync();
		}

        public async Task<List<WorkoutPlanResponse>> GetUserWorkoutPlans(int userId)
        {
            var plans = await _context.WorkoutPlans
                .Include(wp => wp.WorkoutSessions)
                    .ThenInclude(ws => ws.Exercises)
                .Where(wp => wp.UserId == userId)
                .OrderByDescending(wp => wp.CreatedAt)
                .ToListAsync();

            return plans.Select(MapToWorkoutPlanResponse).ToList();
        }

        public async Task<WorkoutPlanResponse> GetWorkoutPlanById(int planId, int userId)
        {
            var plan = await _context.WorkoutPlans
                .Include(wp => wp.WorkoutSessions)
                    .ThenInclude(ws => ws.Exercises.OrderBy(e => e.OrderIndex))
                .FirstOrDefaultAsync(wp => wp.Id == planId && wp.UserId == userId);

            if (plan == null)
            {
                throw new Exception("Plan de entrenamiento no encontrado");
            }

            return MapToWorkoutPlanResponse(plan);
        }

        public async Task<bool> DeleteWorkoutPlan(int planId, int userId)
        {
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(wp => wp.Id == planId && wp.UserId == userId);

            if (plan == null)
            {
                return false;
            }

            _context.WorkoutPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkoutPlanResponse> GenerateAIWorkoutPlan(int userId, CreateWorkoutPlanRequest request)
        {
            // TODO: Implementar integración con IA (OpenAI, Claude, etc.)
            // Por ahora, crear un plan de ejemplo
            var plan = await CreateWorkoutPlan(userId, request);
            
            var workoutPlan = await _context.WorkoutPlans.FindAsync(plan.Id);
            workoutPlan.GeneratedByAI = true;
            workoutPlan.AIPrompt = $"Goal: {request.Goal}, Level: {request.ExperienceLevel}, Style: {request.TrainingStyle}";
            await _context.SaveChangesAsync();

            return plan;
        }

        public async Task LogWorkout(int userId, LogWorkoutRequest request)
        {
            var log = new WorkoutLog
            {
                UserId = userId,
                WorkoutSessionId = request.WorkoutSessionId,
                Date = request.Date,
                DurationMinutes = request.DurationMinutes,
                CaloriesBurned = request.CaloriesBurned,
                Completed = request.Completed,
                Notes = request.Notes,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkoutLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkoutLog>> GetWorkoutLogs(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.WorkoutLogs
                .Include(wl => wl.WorkoutSession)
                .Where(wl => wl.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(wl => wl.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(wl => wl.Date <= endDate.Value);
            }

            return await query.OrderByDescending(wl => wl.Date).ToListAsync();
        }

        private WorkoutPlanResponse MapToWorkoutPlanResponse(WorkoutPlan plan)
        {
            return new WorkoutPlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Goal = plan.Goal,
                ExperienceLevel = plan.ExperienceLevel,
                DurationMinutes = plan.DurationMinutes,
                DaysPerWeek = plan.DaysPerWeek,
                TrainingStyle = plan.TrainingStyle,
                IsActive = plan.IsActive,
                GeneratedByAI = plan.GeneratedByAI,
                CreatedAt = plan.CreatedAt,
                WorkoutSessions = plan.WorkoutSessions?.OrderBy(ws => ws.OrderIndex).Select(ws => new WorkoutSessionResponse
                {
                    Id = ws.Id,
                    DayOfWeek = ws.DayOfWeek,
                    Name = ws.Name,
                    EstimatedCalories = ws.EstimatedCalories,
                    Difficulty = ws.Difficulty,
                    Exercises = ws.Exercises?.OrderBy(e => e.OrderIndex).Select(e => new ExerciseResponse
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        DurationSeconds = e.DurationSeconds,
                        RestSeconds = e.RestSeconds,
                        Sets = e.Sets,
                        Reps = e.Reps,
                        VideoUrl = e.VideoUrl,
                        ImageUrl = e.ImageUrl,
                        Tips = e.Tips
                    }).ToList()
                }).ToList()
            };
        }
    }
}