using Microsoft.AspNetCore.Mvc;
using NutrionE.Util;
using NutrionE.Models.DTOs.User;
using NutrionE.Services.Interfaces;

namespace NutrionE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUsersService usersService) : Controller
    {
        public IUsersService usersService = usersService;
        private string userId = "amzn1.ask.account.AMAXJBIE2P34FE3ZJOCHHO5B2YH4WNXMYV5JWOFI2HRK2AD2GW63V4TSH4HN6AMFLWUYXARCW6FNBWA6PQFH5IENN4TJ54WEBPQMYQTYHZBVIN5YTBCXB4KI4UW35OLSXZVYVK2EBMQ637Z4YTCPHXQMEWZ5GVGGFIWOQMRCIFYL4Z5OHRKW7WQ6VFRPJXLSLNGKRGHSAZ74K7PS3S4ERFVJAA7BUEHAYJLPB7T2GY";

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return(await usersService.GetUser(this.userId)).ToActionResult();
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            var result = await usersService.UpdateUser(user);
            return Ok(result);
        }

    }
}
