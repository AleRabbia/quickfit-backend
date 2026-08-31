using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFit.API.Models.DTOs.Requests;
using QuickFit.API.Services.Interfaces;

namespace QuickFit.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpPost]
        public async Task<IActionResult> AddProgress([FromBody] AddProgressRequest request)
        {
            try
            {
                var userId = GetUserId();
                var progress = await _progressService.AddProgress(userId, request);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProgress([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetUserId();
                var progress = await _progressService.GetUserProgress(userId, startDate, endDate);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestProgress()
        {
            try
            {
                var userId = GetUserId();
                var progress = await _progressService.GetLatestProgress(userId);
                
                if (progress == null)
                {
                    return NotFound(new { message = "No hay registros de progreso" });
                }
                
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{progressId}")]
        public async Task<IActionResult> DeleteProgress(int progressId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _progressService.DeleteProgress(progressId, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Registro no encontrado" });
                }
                
                return Ok(new { message = "Registro eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}