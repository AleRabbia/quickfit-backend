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
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpPost("plans")]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody] CreateWorkoutPlanRequest request)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _workoutService.CreateWorkoutPlan(userId, request);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("plans/generate-ai")]
        public async Task<IActionResult> GenerateAIWorkoutPlan([FromBody] CreateWorkoutPlanRequest request)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _workoutService.GenerateAIWorkoutPlan(userId, request);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("plans")]
        public async Task<IActionResult> GetUserWorkoutPlans()
        {
            try
            {
                var userId = GetUserId();
                var plans = await _workoutService.GetUserWorkoutPlans(userId);
                return Ok(plans);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("plans/{planId}")]
        public async Task<IActionResult> GetWorkoutPlanById(int planId)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _workoutService.GetWorkoutPlanById(planId, userId);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("plans/{planId}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int planId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _workoutService.DeleteWorkoutPlan(planId, userId);
                
                if (!result)
                {
                    return NotFound(new { message = "Plan no encontrado" });
                }
                
                return Ok(new { message = "Plan eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logs")]
        public async Task<IActionResult> LogWorkout([FromBody] LogWorkoutRequest request)
        {
            try
            {
                var userId = GetUserId();
                await _workoutService.LogWorkout(userId, request);
                return Ok(new { message = "Entrenamiento registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetWorkoutLogs([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetUserId();
                var logs = await _workoutService.GetWorkoutLogs(userId, startDate, endDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}