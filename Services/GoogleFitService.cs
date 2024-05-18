using Google.Apis.Auth.OAuth2;
using Google.Apis.Fitness.v1;
using Google.Apis.Fitness.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using NutrionE.Services.Interfaces;

namespace NutrionE.Services
{
    public class GoogleFitService : IGoogleFitService
    {
        private static readonly string[] Scopes = { FitnessService.Scope.FitnessActivityRead };
        private static readonly string ApplicationName = "NutriOne";
        private UserCredential credential;
        private FitnessService service;

        public async Task InitializeAsync(string credentialsPath, string tokenPath)
        {
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                var clientSecrets = GoogleClientSecrets.Load(stream).Secrets;
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true));
            }

            service = new FitnessService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }


        public async Task<IEnumerable<float>> GetCaloriesAsync(DateTime startTime, DateTime endTime)
        {
            if (service == null)
            {
                throw new InvalidOperationException("Service is not initialized. Call InitializeAsync first.");
            }

            var request = service.Users.Dataset.Aggregate(new AggregateRequest
            {
                AggregateBy = new List<AggregateBy>
                {
                    new AggregateBy
                    {
                        DataTypeName = "com.google.calories.expended",
                        DataSourceId = "derived:com.google.calories.expended:com.google.android.gms:merge_calories_expended"
                    }
                },
                BucketByTime = new BucketByTime
                {
                    DurationMillis = 86400000 // 1 day in milliseconds
                },
                StartTimeMillis = GetUnixTimeMillis(startTime),
                EndTimeMillis = GetUnixTimeMillis(endTime)
            }, "me");

            var response = await request.ExecuteAsync();
            var caloriesList = new List<float>();

            if (response?.Bucket != null)
            {
                foreach (var bucket in response.Bucket)
                {
                    if (bucket?.Dataset != null)
                    {
                        foreach (var dataset in bucket.Dataset)
                        {
                            if (dataset?.Point != null)
                            {
                                foreach (var point in dataset.Point)
                                {
                                    if (point?.Value != null)
                                    {
                                        foreach (var value in point.Value)
                                        {
                                            if (value?.FpVal != null)
                                            {
                                                caloriesList.Add((float)value.FpVal);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return caloriesList;
        }

        private long GetUnixTimeMillis(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }
    }
}
