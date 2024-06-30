using NutrionE.Models;

namespace NutrionE.Services.Interfaces
{
    public interface IDailyDietsService
    {

        Task<OperationResult<DailyDiet>> GetDailyDiet(string userId);

        Task<OperationResult<DailyDiet>> CreateDailyDiet(string userId);

    }
}
