using System.Threading.Tasks;
using QuickFit.API.Models.DTOs.Responses;

namespace QuickFit.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsResponse> GetDashboardStats(int userId);
    }
}