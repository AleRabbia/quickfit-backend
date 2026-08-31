using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Models.Entities;

namespace QuickFit.API.Services.Interfaces
{
    public interface IProgressService
    {
        Task<UserProgress> AddProgress(int userId, AddProgressRequest request);
        Task<List<UserProgress>> GetUserProgress(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<UserProgress> GetLatestProgress(int userId);
        Task<bool> DeleteProgress(int progressId, int userId);
    }
}