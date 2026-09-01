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
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpPost("plans")]
        public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanRequest request)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _mealService.CreateMealPlan(userId, request);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("plans/generate-ai")]
        public async Task<IActionResult> GenerateAIMealPlan([FromBody] GenerateAIMealPlanRequest request)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _mealService.GenerateAIMealPlan(userId, request);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("plans")]
        public async Task<IActionResult> GetUserMealPlans()
        {
            try
            {
                var userId = GetUserId();
                var plans = await _mealService.GetUserMealPlans(userId);
                return Ok(plans);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("plans/{planId}")]
        public async Task<IActionResult> GetMealPlanById(int planId)
        {
            try
            {
                var userId = GetUserId();
                var plan = await _mealService.GetMealPlanById(planId, userId);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("plans/{planId}")]
        public async Task<IActionResult> DeleteMealPlan(int planId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _mealService.DeleteMealPlan(planId, userId);
                
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
        public async Task<IActionResult> LogMeal([FromBody] LogMealRequest request)
        {
            try
            {
                var userId = GetUserId();
                await _mealService.LogMeal(userId, request.MealId, request.Date, request.Completed);
                return Ok(new { message = "Comida registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetMealLogs([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var userId = GetUserId();
                var logs = await _mealService.GetMealLogs(userId, startDate, endDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTO para LogMeal
    public class LogMealRequest
    {
        public int MealId { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
    }
}