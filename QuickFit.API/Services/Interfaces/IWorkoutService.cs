using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.DTOs.Responses;
using QuickFit.API.Models.Entities;

namespace QuickFit.API.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<WorkoutPlanResponse> CreateWorkoutPlan(int userId, CreateWorkoutPlanRequest request);
        Task<List<WorkoutPlanResponse>> GetUserWorkoutPlans(int userId);
        Task<WorkoutPlanResponse> GetWorkoutPlanById(int planId, int userId);
        Task<bool> DeleteWorkoutPlan(int planId, int userId);
        Task<WorkoutPlanResponse> GenerateAIWorkoutPlan(int userId, CreateWorkoutPlanRequest request);
        Task LogWorkout(int userId, LogWorkoutRequest request);
        Task<List<WorkoutLog>> GetWorkoutLogs(int userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}