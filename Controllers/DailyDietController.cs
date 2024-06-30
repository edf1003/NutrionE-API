using Microsoft.AspNetCore.Mvc;
using NutrionE.Models;
using NutrionE.Services.Interfaces;
using NutrionE.Util;

namespace NutrionE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DailyDietController(IDailyDietsService dailyDietsService) : Controller
    {
        public IDailyDietsService dailyDietsService = dailyDietsService;
        private string userId = "amzn1.ask.account.AMAXJBIE2P34FE3ZJOCHHO5B2YH4WNXMYV5JWOFI2HRK2AD2GW63V4TSH4HN6AMFLWUYXARCW6FNBWA6PQFH5IENN4TJ54WEBPQMYQTYHZBVIN5YTBCXB4KI4UW35OLSXZVYVK2EBMQ637Z4YTCPHXQMEWZ5GVGGFIWOQMRCIFYL4Z5OHRKW7WQ6VFRPJXLSLNGKRGHSAZ74K7PS3S4ERFVJAA7BUEHAYJLPB7T2GY";

        [HttpGet("GetDailyDiet")]
        public async Task<IActionResult> GetDailyRoutine()
        {
            return (await dailyDietsService.GetDailyDiet(this.userId)).ToActionResult();
        }

        [HttpPost("CreateDailyDiet")]
        public async Task<IActionResult> CreateDailyRoutine()
        {
            return (await dailyDietsService.CreateDailyDiet(this.userId)).ToActionResult();
        }

    }
}
