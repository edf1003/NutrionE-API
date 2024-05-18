using NutrionE.Models;

namespace NutrionE.Services.Interfaces
{
    public interface IChatGPTService
    {
        public Task<string> CreateDailyDiet(string userId);

        public Task<string> CreateDailyRoutine(string userId, ExerciseType exerciseType);


    }
}
