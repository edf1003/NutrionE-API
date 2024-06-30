using Microsoft.AspNetCore.Mvc;
using NutrionE.Services.Interfaces;
namespace NutrionE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlexaController (IAlexaService alexaService, IUsersService usersService) : ControllerBase
    {
        public IAlexaService alexaService = alexaService;
        public IUsersService usersService = usersService;

        #region Diets

        [HttpGet("DailyDiet")]
        public async Task<IActionResult> GetDailyDiet(string userId)
        {
            var result = await alexaService.GetDailyDiet(userId);
            return Ok(result);
        }


        [HttpGet("DailyBreakfast")]
        public async Task<IActionResult> GetDailyBreakfast(string userId)
        {
            var result = await alexaService.GetDailyBreakfast(userId);
            return Ok(result);
        }


        [HttpGet("DailyLunch")]
        public async Task<IActionResult> GetDailyLunch(string userId)
        {
            var result = await alexaService.GetDailyLunch(userId);
            return Ok(result);
        }


        [HttpGet("DailyDinner")]
        public async Task<IActionResult> GetDailyDinner(string userId)
        {
            var result = await alexaService.GetDailyDinner(userId);
            return Ok(result);
        }

        #endregion Diets

        #region Routines

        [HttpGet("DailyRoutine")]
        public async Task<IActionResult> GetDailyRoutine(string userId, string exercise)
        {
            var result = await alexaService.GetDailyRoutine(userId, exercise);
            return Ok(result);
        }

        #endregion Routines

        #region GetUserData

        [HttpGet("GetUserDietType")]
        public async Task<IActionResult> GetUserDietType(string userId)
        {
            var result = await usersService.GetUserDietType(userId);
            return Ok(result);
        }

        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName(string userId)
        {
            var result = await usersService.GetUserName(userId);
            return Ok(result);
        }

        [HttpGet("GetUserAge")]

        public async Task<IActionResult> GetUserAge(string userId)
        {
            var result = await usersService.GetUserAge(userId);
            return Ok(result);
        }

        [HttpGet("GetUserWeight")]
        public async Task<IActionResult> GetUserWeight(string userId)
        {
            var result = await usersService.GetUserWeight(userId);
            return Ok(result);
        }

        [HttpGet("GetUserHeight")]

        public async Task<IActionResult> GetUserHeight(string userId)
        {
            var result = await usersService.GetUserHeight(userId);
            return Ok(result);
        }

        [HttpGet("GetUserGender")]

        public async Task<IActionResult> GetUserGender(string userId)
        {
            var result = await usersService.GetUserGender(userId);
            return Ok(result);
        }

        #endregion GetUserData

        #region UpdateUserData


        [HttpPost("UpdateUserName")]
        public async Task<IActionResult> UpdateUserName(string userId, string name)
        {
            await usersService.UpdateUserName(userId, name);
            return Ok();
        }

        [HttpPost("UpdateUserAge")]
        public async Task<IActionResult> UpdateUserAge(string userId, string age)
        {
            await usersService.UpdateUserAge(userId, age);
            return Ok();
        }

        [HttpPost("UpdateUserWeight")]
        public async Task<IActionResult> UpdateUserWeight(string userId, string weight)
        {
            await usersService.UpdateUserWeight(userId, weight);
            return Ok();
        }

        [HttpPost("UpdateUserHeight")]

        public async Task<IActionResult> UpdateUserHeight(string userId, string height)
        {
            await usersService.UpdateUserHeight(userId, height);
            return Ok();
        }

        [HttpPost("UpdateUserGender")]
        public async Task<IActionResult> UpdateUserGender(string userId, string gender)
        {
            await usersService.UpdateUserGender(userId, gender);
            return Ok();
        }

        [HttpPost("UpdateUserDietType")]
        public async Task<IActionResult> UpdateUserDietType(string userId, string dietType)
        {
            await usersService.UpdateUserDietType(userId, dietType);
            return Ok();
        }

        [HttpPost("UpdateUserObjective")]
        public async Task<IActionResult> UpdateUserObjective(string userId, string objective)
        {
                await usersService.UpdateUserObjective(userId, objective);
            return Ok();
        }

        [HttpPost("UpdateUserMail")]
        public async Task<IActionResult> UpdateUserMail(string userId, string mail)
        {
            await usersService.UpdateUserMail(userId, mail);
            return Ok();
        }

        #endregion UpdateUserData

        
    }

}
