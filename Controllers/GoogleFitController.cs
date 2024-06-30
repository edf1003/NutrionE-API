using Microsoft.AspNetCore.Mvc;
using NutrionE.Services;
using NutrionE.Services.Interfaces;
using NutrionE.Util;

namespace NutrionE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleFitController(IGoogleFitService googleFitService) : Controller
    {
        public IGoogleFitService googleFitService = googleFitService;

        [HttpGet("Access")]
        public IActionResult Access()
        {
            var authorizationUrl = googleFitService.GetAuthorizationUrl();
            return Redirect(authorizationUrl);
        }

        [HttpGet("oauth2callback")]
        public async Task<IActionResult> OAuth2Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code is missing");
            }

            await googleFitService.ExchangeCodeForTokensAsync(code);
            return Ok();
        }

        [HttpGet("GetDailyCaloriesForLastWeek")]
        public async Task<IActionResult> GetCalories()
        {
            return (await googleFitService.GetCaloriesLastWeekAsync()).ToActionResult();
        }

        [HttpGet("GetDailyStepsForLastWeek")]
        public async Task<IActionResult> GetDailyStepsForLastWeek()
        {
            return (await googleFitService.GetDailyStepsForLastWeekAsync()).ToActionResult();
        }


    }
}
