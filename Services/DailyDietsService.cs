using Microsoft.EntityFrameworkCore;
using NutrionE.Models;
using NutrionE.Services.Interfaces;

namespace NutrionE.Services
{
    public class DailyDietsService(NutrioneContext context, IChatGPTService chatGPTService) : IDailyDietsService
    {

        private readonly NutrioneContext context = context;
        private readonly IChatGPTService chatGPTService = chatGPTService;

        public async Task<OperationResult<DailyDiet>> GetDailyDiet(string userId)
        {
            var dailyDate = await context.DailyDiets
                .Include(dd => dd.Meals)
                .Where(dd => dd.UserId == userId)
                .OrderByDescending(dr => dr.Id)
                .FirstOrDefaultAsync();

            if (dailyDate == null)
            {
                return OperationResult<DailyDiet>.Success(new());
            }
            return OperationResult<DailyDiet>.Success(dailyDate);
        }

        public async Task<OperationResult<DailyDiet>> CreateDailyDiet(string userId)
        {
            _ = await chatGPTService.CreateDailyDiet(userId);
            return await GetDailyDiet(userId);
        }
    }
}
