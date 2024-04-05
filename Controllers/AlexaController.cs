using Microsoft.AspNetCore.Mvc;
using NutrionE.Services;
namespace NutrionE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlexaController : ControllerBase
    {
        public AlexaService alexaService;
        public AlexaController(AlexaService alexaService)
        {
            this.alexaService = alexaService;
        }



    }
}
