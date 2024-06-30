using NutrionE.Models;
using static NutrionE.Services.GoogleFitService;

namespace NutrionE.Services.Interfaces
{
    public interface IGoogleFitService
    {
        public string GetAuthorizationUrl();

        public Task ExchangeCodeForTokensAsync(string code);

        public Task<OperationResult<List<double>>> GetCaloriesLastWeekAsync();

        public Task<OperationResult<List<int>>> GetDailyStepsForLastWeekAsync();
    }
}
