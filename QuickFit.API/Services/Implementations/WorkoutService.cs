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
        private readonly IGeminiService _geminiService;

        public WorkoutService(QuickFitDbContext context, IGeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
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

            Console.WriteLine($"Name: {workoutPlan.Name} ({workoutPlan.Name?.Length})");
            Console.WriteLine($"Goal: {workoutPlan.Goal} ({workoutPlan.Goal?.Length})");
            Console.WriteLine($"ExperienceLevel: {workoutPlan.ExperienceLevel} ({workoutPlan.ExperienceLevel?.Length})");
            Console.WriteLine($"TrainingStyle: {workoutPlan.TrainingStyle} ({workoutPlan.TrainingStyle?.Length})");
            Console.WriteLine($"TrainingPlace: {workoutPlan.TrainingPlace} ({workoutPlan.TrainingPlace?.Length})");
            Console.WriteLine($"TrainingFocus: {workoutPlan.TrainingFocus} ({workoutPlan.TrainingFocus?.Length})");
            
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
            if (string.IsNullOrWhiteSpace(request.Goal) ||
                string.IsNullOrWhiteSpace(request.ExperienceLevel) ||
                string.IsNullOrWhiteSpace(request.TrainingStyle) ||
                string.IsNullOrWhiteSpace(request.TrainingPlace) ||
                string.IsNullOrWhiteSpace(request.TrainingFocus))
            {
                throw new Exception("Faltan datos obligatorios para generar el plan con IA.");
            }

            var trainingDays = request.TrainingDays != null && request.TrainingDays.Any()
                ? request.TrainingDays
                : GetDefaultTrainingDays();

            var aiJson = await _geminiService.GenerateWorkoutPlanJsonAsync(request);

            if (string.IsNullOrWhiteSpace(aiJson))
            {
                throw new Exception("Gemini no devolvió un plan válido.");
            }

            var cleanJson = aiJson.Trim();
            if (cleanJson.StartsWith("```"))
            {
                cleanJson = cleanJson.Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Replace("```", string.Empty, StringComparison.OrdinalIgnoreCase)
                    .Trim();
            }

            var aiPlan = JsonSerializer.Deserialize<GeminiWorkoutPlanResponse>(cleanJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (aiPlan == null)
            {
                throw new Exception("No se pudo interpretar la respuesta de Gemini.");
            }

            var plan = new WorkoutPlan
            {
                UserId = userId,
                Name = string.IsNullOrWhiteSpace(aiPlan.Name) ? $"Plan IA - {GetGoalLabel(request.Goal)}" : aiPlan.Name,
                Description = string.IsNullOrWhiteSpace(aiPlan.Description)
                    ? $"Plan generado por Gemini para {GetGoalLabel(request.Goal).ToLowerInvariant()} con enfoque {request.TrainingFocus}."
                    : aiPlan.Description,
                Goal = string.IsNullOrWhiteSpace(aiPlan.Goal) ? request.Goal : aiPlan.Goal,
                ExperienceLevel = string.IsNullOrWhiteSpace(aiPlan.ExperienceLevel) ? request.ExperienceLevel : aiPlan.ExperienceLevel,
                DurationMinutes = aiPlan.DurationMinutes > 0 ? aiPlan.DurationMinutes : (request.DurationMinutes > 0 ? request.DurationMinutes : 30),
                DaysPerWeek = aiPlan.DaysPerWeek > 0 ? aiPlan.DaysPerWeek : trainingDays.Count,
                TrainingStyle = string.IsNullOrWhiteSpace(aiPlan.TrainingStyle) ? request.TrainingStyle : aiPlan.TrainingStyle,
                TrainingPlace = string.IsNullOrWhiteSpace(aiPlan.TrainingPlace) ? request.TrainingPlace : aiPlan.TrainingPlace,
                Equipment = JsonSerializer.Serialize(aiPlan.Equipment ?? request.Equipment ?? new List<string>()),
                MedicalHistory = request.MedicalHistory,
                DislikedExercises = request.DislikedExercises,
                TrainingFocus = string.IsNullOrWhiteSpace(aiPlan.TrainingFocus) ? request.TrainingFocus : aiPlan.TrainingFocus,
                IsActive = true,
                GeneratedByAI = true,
                AIPrompt = $"Goal: {request.Goal}, Level: {request.ExperienceLevel}, Style: {request.TrainingStyle}, Days: {string.Join(", ", trainingDays)}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(plan);
            await _context.SaveChangesAsync();

            var sessions = new List<WorkoutSession>();
            var aiSessions = aiPlan.Sessions ?? new List<GeminiWorkoutSessionResponse>();

            for (var i = 0; i < aiSessions.Count; i++)
            {
                var sessionModel = aiSessions[i];
                var session = new WorkoutSession
                {
                    WorkoutPlanId = plan.Id,
                    DayOfWeek = string.IsNullOrWhiteSpace(sessionModel.DayOfWeek) ? trainingDays[i % trainingDays.Count] : sessionModel.DayOfWeek,
                    Name = string.IsNullOrWhiteSpace(sessionModel.Name) ? $"Sesión {i + 1}" : sessionModel.Name,
                    EstimatedCalories = sessionModel.EstimatedCalories > 0 ? sessionModel.EstimatedCalories : EstimateCalories(request.Goal, request.ExperienceLevel, request.DurationMinutes),
                    Difficulty = string.IsNullOrWhiteSpace(sessionModel.Difficulty) ? GetDifficultyLabel(request.ExperienceLevel) : sessionModel.Difficulty,
                    OrderIndex = i,
                    CreatedAt = DateTime.UtcNow
                };

                sessions.Add(session);
            }

            _context.WorkoutSessions.AddRange(sessions);
            await _context.SaveChangesAsync();

            foreach (var session in sessions)
            {
                var sessionExercises = aiSessions.FirstOrDefault(s =>
                    s.DayOfWeek == session.DayOfWeek ||
                    s.Name == session.Name) ?.Exercises ?? new List<GeminiWorkoutExerciseResponse>();

                foreach (var exercise in sessionExercises.Select((item, index) => new { item, index }))
                {
                    _context.Exercises.Add(new Exercise
                    {
                        WorkoutSessionId = session.Id,
                        Name = string.IsNullOrWhiteSpace(exercise.item.Name) ? "Ejercicio" : exercise.item.Name,
                        Description = exercise.item.Description,
                        DurationSeconds = exercise.item.DurationSeconds > 0 ? exercise.item.DurationSeconds : 45,
                        RestSeconds = exercise.item.RestSeconds > 0 ? exercise.item.RestSeconds : 15,
                        Sets = exercise.item.Sets > 0 ? exercise.item.Sets : 3,
                        Reps = exercise.item.Reps > 0 ? exercise.item.Reps : 12,
                        OrderIndex = exercise.index,
                        Tips = exercise.item.Tips,
                        VideoUrl = null,
                        ImageUrl = null,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();

            return await GetWorkoutPlanById(plan.Id, userId);
        }

        private async Task CreateAIWorkoutSessions(int workoutPlanId, CreateWorkoutPlanRequest request, List<string> trainingDays)
        {
            var sessions = new List<WorkoutSession>();

            for (var i = 0; i < trainingDays.Count; i++)
            {
                var day = trainingDays[i];
                var sessionName = BuildSessionName(request.Goal, day, request.TrainingFocus);

                sessions.Add(new WorkoutSession
                {
                    WorkoutPlanId = workoutPlanId,
                    DayOfWeek = day,
                    Name = sessionName,
                    EstimatedCalories = EstimateCalories(request.Goal, request.ExperienceLevel, request.DurationMinutes),
                    Difficulty = GetDifficultyLabel(request.ExperienceLevel),
                    OrderIndex = i,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _context.WorkoutSessions.AddRange(sessions);
            await _context.SaveChangesAsync();

            foreach (var session in sessions)
            {
                var exercises = BuildAIExercises(request, session.DayOfWeek);
                _context.Exercises.AddRange(exercises.Select((exercise, index) => new Exercise
                {
                    WorkoutSessionId = session.Id,
                    Name = exercise.Name,
                    Description = exercise.Description,
                    DurationSeconds = exercise.DurationSeconds,
                    RestSeconds = exercise.RestSeconds,
                    Sets = exercise.Sets,
                    Reps = exercise.Reps,
                    OrderIndex = index,
                    Tips = exercise.Tips,
                    VideoUrl = null,
                    ImageUrl = null,
                    CreatedAt = DateTime.UtcNow
                }));
            }

            await _context.SaveChangesAsync();
        }

        private static List<ExerciseTemplate> BuildAIExercises(CreateWorkoutPlanRequest request, string dayOfWeek)
        {
            var goal = (request.Goal ?? "maintain").ToLowerInvariant();
            var hasEquipment = request.Equipment != null && request.Equipment.Any();
            var equipmentText = hasEquipment ? string.Join(", ", request.Equipment) : "peso corporal";

            return goal switch
            {
                "lose_weight" or "cardio" => new List<ExerciseTemplate>
                {
                    new("Salto de tijera", "Movilidad y fuerza cardiovascular", 40, 20, 3, 12, "Mantén la columna neutra y acelera el ritmo de forma controlada."),
                    new("Sentadilla corporal", "Fortalece piernas y glúteos", 45, 15, 3, 15, "Y no dejes que los tobillos se inclinen hacia dentro."),
                    new("Plancha frontal", "Active el core y estabiliza la cadera", 40, 20, 3, 1, "Mantén el torso recto y la cintura firme."),
                    new("Mountain climbers", "Cardio con trabajo de core y coordinación", 45, 15, 3, 20, "Trae la rodilla hacia el pecho sin arquear la espalda."),
                    new("Burpees", "Circuito completo de intensidad", 45, 20, 3, 10, "Usa la respiración para mantener la intensidad sostenida.")
                },
                "gain_muscle" or "strength" => new List<ExerciseTemplate>
                {
                    new("Sentadilla goblet", $"Base de fuerza con {equipmentText}", 50, 20, 4, 10, "Mantén pecho alto y la rodilla alineada con el pie."),
                    new("Press de pecho", $"Empuje superior con {equipmentText}", 50, 20, 4, 8, "Controla la bajada para proteger hombros y codos."),
                    new("Peso muerto rumano", "Trabajo posterior y fuerza de piernas", 50, 20, 4, 10, "No redondees la espalda; empuja con glúteos."),
                    new("Dominadas o remos", $"Fuerza de espalda con {equipmentText}", 50, 20, 3, 8, "Ajusta el rango para evitar tensión innecesaria."),
                    new("Plancha con leva", "Estabilidad y core de fuerza", 40, 15, 3, 12, "Haz la transferencia de peso sin perder la línea corporal.")
                },
                "flexibility" or "mobility" => new List<ExerciseTemplate>
                {
                    new("Estiramiento de cadera", "Movilidad y apertura pélvica", 45, 15, 2, 10, "Respira profundo y mueve con control."),
                    new("Puente de glúteos", "Activación y apertura de la parte posterior", 45, 20, 3, 12, "Eleva sin forzar la zona lumbar."),
                    new("Balance de una pierna", "Estabilidad y coordinación", 40, 20, 3, 8, "Mantén la pelvis nivelada con toda la postura."),
                    new("Rotación torácica", "Movilidad del tronco y hombros", 35, 15, 2, 10, "Mueve el torso sin forzar el cuello."),
                    new("Stretching de piernas", "Recuperación y elasticidad final", 50, 15, 2, 1, "Mantén cada estiramiento entre 20 y 30 segundos.")
                },
                _ => new List<ExerciseTemplate>
                {
                    new("Circuito de fuerza general", "Trabajo completo y balanceado", 40, 20, 3, 12, "Mantén un tempo constante y buen control técnico."),
                    new("Sentadilla a caja", "Potencia y estabilidad de piernas", 45, 15, 3, 12, "Alinea rodilla, cadera y tobillo."),
                    new("Flexiones o empuje", "Pecho, hombros y triceps", 40, 20, 3, 10, "Baja controlando y empuja con el torso estable."),
                    new("Zancada con giro", "Core y fuerza funcional", 40, 20, 3, 10, "Pisa fuerte y gira con control."),
                    new("Dead bug", "Core profundo y postura", 35, 15, 3, 12, "Mantén la espalda baja pegada al suelo.")
                }
            };
        }

        private static List<string> GetDefaultTrainingDays()
        {
            return new List<string> { "Lunes", "Miércoles", "Viernes" };
        }

        private static string GetGoalLabel(string goal)
        {
            return (goal ?? "maintain").ToLowerInvariant() switch
            {
                "lose_weight" => "Pérdida de peso",
                "gain_muscle" => "Ganancia muscular",
                "cardio" => "Resistencia",
                "flexibility" => "Flexibilidad",
                "maintain" => "Mantenimiento",
                _ => "Objetivo general"
            };
        }

        private static string GetExperienceLabel(string experienceLevel)
        {
            return (experienceLevel ?? "beginner").ToLowerInvariant() switch
            {
                "beginner" => "principiante",
                "intermediate" => "intermedio",
                "advanced" => "avanzado",
                _ => "general"
            };
        }

        private static string GetDifficultyLabel(string experienceLevel)
        {
            return (experienceLevel ?? "beginner").ToLowerInvariant() switch
            {
                "advanced" => "Avanzado",
                "intermediate" => "Intermedio",
                _ => "Principiante"
            };
        }

        private static string BuildSessionName(string goal, string day, string trainingFocus)
        {
            var goalLabel = GetGoalLabel(goal);
            var focusLabel = string.IsNullOrWhiteSpace(trainingFocus) ? "general" : trainingFocus;
            return $"{day} · {goalLabel} · {focusLabel}";
        }

        private static int EstimateCalories(string goal, string experienceLevel, int durationMinutes)
        {
            var baseCalories = durationMinutes * 5;
            var goalBonus = (goal ?? "maintain").ToLowerInvariant() switch
            {
                "lose_weight" or "cardio" => 10,
                "gain_muscle" => 15,
                "flexibility" => 5,
                _ => 8
            };

            var levelBonus = (experienceLevel ?? "beginner").ToLowerInvariant() switch
            {
                "advanced" => 20,
                "intermediate" => 10,
                _ => 0
            };

            return baseCalories + goalBonus + levelBonus;
        }

        private sealed class ExerciseTemplate
        {
            public ExerciseTemplate(string name, string description, int durationSeconds, int restSeconds, int sets, int reps, string tips)
            {
                Name = name;
                Description = description;
                DurationSeconds = durationSeconds;
                RestSeconds = restSeconds;
                Sets = sets;
                Reps = reps;
                Tips = tips;
            }

            public string Name { get; }
            public string Description { get; }
            public int DurationSeconds { get; }
            public int RestSeconds { get; }
            public int Sets { get; }
            public int Reps { get; }
            public string Tips { get; }
        }

        private sealed class GeminiWorkoutPlanResponse
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Goal { get; set; }
            public string? ExperienceLevel { get; set; }
            public int DurationMinutes { get; set; }
            public int DaysPerWeek { get; set; }
            public string? TrainingStyle { get; set; }
            public string? TrainingPlace { get; set; }
            public List<string>? Equipment { get; set; }
            public string? TrainingFocus { get; set; }
            public List<GeminiWorkoutSessionResponse>? Sessions { get; set; }
        }

        private sealed class GeminiWorkoutSessionResponse
        {
            public string? DayOfWeek { get; set; }
            public string? Name { get; set; }
            public int EstimatedCalories { get; set; }
            public string? Difficulty { get; set; }
            public List<GeminiWorkoutExerciseResponse>? Exercises { get; set; }
        }

        private sealed class GeminiWorkoutExerciseResponse
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int DurationSeconds { get; set; }
            public int RestSeconds { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
            public string? Tips { get; set; }
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