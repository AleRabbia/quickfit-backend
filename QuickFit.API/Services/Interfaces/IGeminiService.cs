using QuickFit.API.Models.DTOs.Requests;

namespace QuickFit.API.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string?> GenerateWorkoutPlanJsonAsync(CreateWorkoutPlanRequest request);
    }
}
