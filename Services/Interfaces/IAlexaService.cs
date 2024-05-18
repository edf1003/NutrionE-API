namespace NutrionE.Services.Interfaces
{
    public interface IAlexaService
    {
        Task<string> GetDailyDiet(string userId);

        Task<string> GetDailyBreakfast(string userId);

        Task<string> GetDailyLunch(string userId);

        Task<string> GetDailyDinner(string userId);

        Task<string> GetDailyRoutine(string userId, string exercise);

    }
}
