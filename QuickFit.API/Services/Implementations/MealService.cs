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
    public class MealService : IMealService
    {
        private readonly QuickFitDbContext _context;

        public MealService(QuickFitDbContext context)
        {
            _context = context;
        }

        public async Task<MealPlanResponse> CreateMealPlan(int userId, CreateMealPlanRequest request)
        {
            var mealPlan = new MealPlan
            {
                UserId = userId,
                Name = request.Name,
                Description = request.Description,
                Goal = request.Goal,
                DietType = request.DietType,
                DailyCalories = request.DailyCalories,
                DailyProtein = request.DailyProtein,
                DailyCarbs = request.DailyCarbs,
                DailyFats = request.DailyFats,
                MealsPerDay = request.MealsPerDay,
                Allergies = JsonSerializer.Serialize(request.Allergies ?? new List<string>()),
                Intolerances = JsonSerializer.Serialize(request.Intolerances ?? new List<string>()),
                DislikedFoods = JsonSerializer.Serialize(request.DislikedFoods ?? new List<string>()),
                Budget = request.Budget,
                CookingTime = request.CookingTime,
                IsActive = true,
                GeneratedByAI = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MealPlans.Add(mealPlan);
            await _context.SaveChangesAsync();

            // Crear comidas de ejemplo
            await CreateSampleMeals(mealPlan.Id, request.MealsPerDay, request.DailyCalories);

            return await GetMealPlanById(mealPlan.Id, userId);
        }

        private async Task CreateSampleMeals(int mealPlanId, int mealsPerDay, int dailyCalories)
        {
            var mealTypes = new[] { "Desayuno", "Media Mañana", "Almuerzo", "Merienda", "Cena" };
            var caloriesPerMeal = dailyCalories / mealsPerDay;

            var meals = new List<Meal>();
            for (int i = 0; i < Math.Min(mealsPerDay, mealTypes.Length); i++)
            {
                var meal = new Meal
                {
                    MealPlanId = mealPlanId,
                    MealType = mealTypes[i],
                    Name = $"{mealTypes[i]} Saludable",
                    Description = $"Comida balanceada para {mealTypes[i].ToLower()}",
                    TimeOfDay = new TimeSpan(7 + (i * 3), 0, 0),
                    Calories = caloriesPerMeal,
                    Protein = caloriesPerMeal / 15,
                    Carbs = caloriesPerMeal / 10,
                    Fats = caloriesPerMeal / 25,
                    OrderIndex = i,
                    CreatedAt = DateTime.UtcNow
                };

                meals.Add(meal);
            }

            _context.Meals.AddRange(meals);
            await _context.SaveChangesAsync();

            // Agregar alimentos de ejemplo
            foreach (var meal in meals)
            {
                await CreateSampleMealFoods(meal.Id);
            }
        }

        private async Task CreateSampleMealFoods(int mealId)
        {
            var foods = new List<MealFood>
            {
                new MealFood
                {
                    MealId = mealId,
                    FoodName = "Avena integral",
                    Quantity = "1 taza",
                    Calories = 150,
                    Protein = 5,
                    Carbs = 27,
                    Fats = 3,
                    OrderIndex = 0,
                    CreatedAt = DateTime.UtcNow
                },
                new MealFood
                {
                    MealId = mealId,
                    FoodName = "Banana",
                    Quantity = "1 unidad",
                    Calories = 100,
                    Protein = 1,
                    Carbs = 27,
                    Fats = 0,
                    OrderIndex = 1,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _context.MealFoods.AddRange(foods);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MealPlanResponse>> GetUserMealPlans(int userId)
        {
            var plans = await _context.MealPlans
                .Include(mp => mp.Meals)
                    .ThenInclude(m => m.MealFoods)
                .Where(mp => mp.UserId == userId)
                .OrderByDescending(mp => mp.CreatedAt)
                .ToListAsync();

            return plans.Select(MapToMealPlanResponse).ToList();
        }

        public async Task<MealPlanResponse> GetMealPlanById(int planId, int userId)
        {
            var plan = await _context.MealPlans
                .Include(mp => mp.Meals.OrderBy(m => m.OrderIndex))
                    .ThenInclude(m => m.MealFoods.OrderBy(mf => mf.OrderIndex))
                .FirstOrDefaultAsync(mp => mp.Id == planId && mp.UserId == userId);

            if (plan == null)
            {
                throw new Exception("Plan nutricional no encontrado");
            }

            return MapToMealPlanResponse(plan);
        }

        public async Task<bool> DeleteMealPlan(int planId, int userId)
        {
            var plan = await _context.MealPlans
                .FirstOrDefaultAsync(mp => mp.Id == planId && mp.UserId == userId);

            if (plan == null)
            {
                return false;
            }

            _context.MealPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MealPlanResponse> GenerateAIMealPlan(int userId, CreateMealPlanRequest request)
        {
            // TODO: Implementar integración con IA
            var plan = await CreateMealPlan(userId, request);
            
            var mealPlan = await _context.MealPlans.FindAsync(plan.Id);
            mealPlan.GeneratedByAI = true;
            mealPlan.AIPrompt = $"Goal: {request.Goal}, Diet: {request.DietType}, Calories: {request.DailyCalories}";
            await _context.SaveChangesAsync();

            return plan;
        }

        public async Task LogMeal(int userId, int mealId, DateTime date, bool completed)
        {
            var meal = await _context.Meals.FindAsync(mealId);
            if (meal == null)
            {
                throw new Exception("Comida no encontrada");
            }

            var log = new MealLog
            {
                UserId = userId,
                MealId = mealId,
                Date = date,
                MealType = meal.MealType,
                Completed = completed,
                CreatedAt = DateTime.UtcNow
            };

            _context.MealLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MealLog>> GetMealLogs(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.MealLogs
                .Include(ml => ml.Meal)
                .Where(ml => ml.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(ml => ml.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(ml => ml.Date <= endDate.Value);
            }

            return await query.OrderByDescending(ml => ml.Date).ToListAsync();
        }

        private MealPlanResponse MapToMealPlanResponse(MealPlan plan)
        {
            return new MealPlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Goal = plan.Goal,
                DietType = plan.DietType,
                DailyCalories = plan.DailyCalories,
                DailyProtein = plan.DailyProtein,
                DailyCarbs = plan.DailyCarbs,
                DailyFats = plan.DailyFats,
                MealsPerDay = plan.MealsPerDay,
                IsActive = plan.IsActive,
                GeneratedByAI = plan.GeneratedByAI,
                CreatedAt = plan.CreatedAt,
                Meals = plan.Meals?.OrderBy(m => m.OrderIndex).Select(m => new MealResponse
                {
                    Id = m.Id,
                    MealType = m.MealType,
                    Name = m.Name,
                    Description = m.Description,
                    TimeOfDay = m.TimeOfDay?.ToString(@"hh\:mm"),
                    Calories = m.Calories,
                    Protein = m.Protein,
                    Carbs = m.Carbs,
                    Fats = m.Fats,
                    RecipeUrl = m.RecipeUrl,
                    ImageUrl = m.ImageUrl,
                    Foods = m.MealFoods?.OrderBy(mf => mf.OrderIndex).Select(mf => new MealFoodResponse
                    {
                        Id = mf.Id,
                        FoodName = mf.FoodName,
                        Quantity = mf.Quantity,
                        Calories = mf.Calories,
                        Protein = mf.Protein,
                        Carbs = mf.Carbs,
                        Fats = mf.Fats
                    }).ToList()
                }).ToList()
            };
        }
    }
}