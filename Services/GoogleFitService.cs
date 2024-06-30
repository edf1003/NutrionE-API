using Newtonsoft.Json;
using NutrionE.Models;
using NutrionE.Models.DTOs.GoogleFitResponsesDTO;
using NutrionE.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using static NutrionE.Services.GoogleFitService;

namespace NutrionE.Services
{
    public class GoogleFitService : IGoogleFitService
    {
        private readonly string clientId = "";
        private readonly string clientSecret = "";
        private readonly string redirectUri = "https://pclp3skl-5129.euw.devtunnels.ms/api/GoogleFit/oauth2callback";
        private readonly HttpClient httpClient;
        private readonly string tokenPath = "./../NutrionE/Configuration/StaticContent/token.txt";

        public GoogleFitService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public string GetAuthorizationUrl()
        {
            var queryParams = new Dictionary<string, string>
            {
            { "client_id", clientId },
            { "redirect_uri", redirectUri },
            { "response_type", "code" },
            { "scope", "https://www.googleapis.com/auth/fitness.activity.read" },
            { "access_type", "offline" }
            };

            var queryString = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            return $"https://accounts.google.com/o/oauth2/v2/auth?{queryString}";
        }

        public async Task ExchangeCodeForTokensAsync(string code)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            })
            };

            var response = await this.httpClient.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            if (tokenData != null && tokenData.TryGetValue("access_token", out var tokenValue))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(tokenPath))
                    {
                        // Escribe el valor en el archivo
                        writer.WriteLine(tokenValue);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else
            {
                throw new InvalidOperationException("Access token not found in response.");
            }
        }


        public async Task<OperationResult<List<double>>> GetCaloriesLastWeekAsync()
        {
            string token = await File.ReadAllTextAsync(tokenPath);
            token = token.Replace("\r", "").Replace("\n", "");

            var baseUrl = "https://www.googleapis.com/fitness/v1/users/me/dataset:aggregate";
            var dataSource = "derived:com.google.calories.expended:com.google.android.gms:merge_calories_expended";

            var today = DateTime.Now;
            var oneWeekAgo = today.Subtract(TimeSpan.FromDays(7));
            var startTimeMillis = (long)(oneWeekAgo - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var endTimeMillis = (long)(today - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var requestBody = JsonConvert.SerializeObject(new
            {
                aggregateBy = new[]
                {
        new
        {
            dataSourceId = dataSource
        }
        },
                bucketByTime = new
                {
                    durationMillis = 86400000
                },
                startTimeMillis = startTimeMillis,
                endTimeMillis = endTimeMillis
            });

            var request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            StepData calorieData = JsonConvert.DeserializeObject<StepData>(responseContent);

            List<double> calorieValues = new List<double>();

            foreach (var bucket in calorieData.bucket)
            {
                if (bucket != null && bucket.dataset != null)
                {
                    foreach (var dataset in bucket.dataset)
                    {
                        if (dataset != null && dataset.point != null)
                        {
                            foreach (var point in dataset.point)
                            {
                                if (point != null && point.value != null)
                                {
                                    foreach (var value in point.value)
                                    {
                                        calorieValues.Add(value.fpVal); 
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return OperationResult<List<double>>.Success(calorieValues);
        }


            public async Task<OperationResult<List<int>>> GetDailyStepsForLastWeekAsync()
        {
            string token = await File.ReadAllTextAsync(tokenPath);
            token = token.Replace("\r", "").Replace("\n", "");

            var baseUrl = "https://www.googleapis.com/fitness/v1/users/me/dataset:aggregate";
            var dataSource = "derived:com.google.step_count.delta:com.google.android.gms:estimated_steps";

            var today = DateTime.Now;
            var oneWeekAgo = today.Subtract(TimeSpan.FromDays(7));
            var startTimeMillis = (long)(oneWeekAgo - new DateTime(1970, 1, 1)).TotalMilliseconds;
            var endTimeMillis = (long)(today - new DateTime(1970, 1, 1)).TotalMilliseconds;

            var requestBody = JsonConvert.SerializeObject(new
            {
                aggregateBy = new[]
                {
            new
            {
                dataSourceId = dataSource
            }
        },
                bucketByTime = new
                {
                    durationMillis = 86400000
                },
                startTimeMillis = startTimeMillis,
                endTimeMillis = endTimeMillis
            });

            var request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            StepData stepData = JsonConvert.DeserializeObject<StepData>(responseContent);

            List<int> intVals = new List<int>();

            foreach (var bucket in stepData.bucket)
            {
                if (bucket != null && bucket.dataset != null)
                {
                    foreach (var dataset in bucket.dataset)
                    {
                        if (dataset != null && dataset.point != null)
                        {
                            foreach (var point in dataset.point)
                            {
                                if (point != null && point.value != null)
                                {
                                    foreach (var value in point.value)
                                    {
                                        intVals.Add(value.intVal);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return OperationResult<List<int>>.Success(intVals);
        }

        public class CaloriesData
        {
            public int Calories { get; set; }
        }
    }
}
