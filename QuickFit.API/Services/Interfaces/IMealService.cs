using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.DTOs.Responses;
using QuickFit.API.Models.Entities;

namespace QuickFit.API.Services.Interfaces
{
    public interface IMealService
    {
        Task<MealPlanResponse> CreateMealPlan(int userId, CreateMealPlanRequest request);
        Task<List<MealPlanResponse>> GetUserMealPlans(int userId);
        Task<MealPlanResponse> GetMealPlanById(int planId, int userId);
        Task<bool> DeleteMealPlan(int planId, int userId);
        Task<MealPlanResponse> GenerateAIMealPlan(int userId, GenerateAIMealPlanRequest request);
        Task LogMeal(int userId, int mealId, DateTime date, bool completed);
        Task<List<MealLog>> GetMealLogs(int userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}