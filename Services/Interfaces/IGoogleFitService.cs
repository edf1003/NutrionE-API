namespace NutrionE.Services.Interfaces
{
    public interface IGoogleFitService
    {
        public Task InitializeAsync(string credentialsPath, string tokenPath);
        public Task<IEnumerable<float>> GetCaloriesAsync(DateTime startTime, DateTime endTime);
    }
}
