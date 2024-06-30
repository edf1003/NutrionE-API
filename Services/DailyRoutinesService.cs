using Microsoft.EntityFrameworkCore;
using NutrionE.Models;
using NutrionE.Services.Interfaces;

namespace NutrionE.Services
{
    public class DailyRoutinesService (NutrioneContext context, IChatGPTService chatGPTService) : IDailyRoutinesService
    {

        private readonly NutrioneContext context = context;
        private readonly IChatGPTService chatGPTService = chatGPTService;

        public async Task<OperationResult<DailyRoutine>> GetDailyRoutine(string userId)
        {
            var dailyRoutine = await context.DailyRoutines
                .Include(dr => dr.Exercises)
                .Where(dr => dr.UserId == userId)
                .OrderByDescending(dr => dr.Id)
                .FirstOrDefaultAsync();

            if (dailyRoutine == null)
            {
                return OperationResult<DailyRoutine>.Success(new());
            }
            return OperationResult<DailyRoutine>.Success(dailyRoutine);
        }

        public async Task<OperationResult<DailyRoutine>> CreateDailyRoutine(string userId, ExerciseType exerciseType)
        {
            _ = await chatGPTService.CreateDailyRoutine(userId, exerciseType);
            return await GetDailyRoutine(userId);
        }
    }
}
