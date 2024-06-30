using NutrionE.Models;

namespace NutrionE.Services.Interfaces
{
    public interface IDailyRoutinesService
    {

        Task<OperationResult<DailyRoutine>> GetDailyRoutine(string userId);

        Task<OperationResult<DailyRoutine>> CreateDailyRoutine(string userId, ExerciseType exerciseType);
    }
}
