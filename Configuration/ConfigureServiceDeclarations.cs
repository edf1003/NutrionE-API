using NutrionE.Services;
using NutrionE.Services.Interfaces;

namespace NutrionE.Configuration
{
    public static class ConfigureServiceDeclarations
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            //Register services here
            services.AddScoped<IAlexaService, AlexaService>();
            services.AddScoped<IChatGPTService, ChatGPTService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGoogleFitService, GoogleFitService>();
            services.AddScoped<IDailyRoutinesService, DailyRoutinesService>();
            services.AddScoped<IDailyDietsService, DailyDietsService>();
            services.AddSingleton(configuration);
            services.AddHttpClient("Auth", x => { x.Timeout = TimeSpan.FromMinutes(10); });
            services.AddHttpClient<GoogleFitService>();
        }

    }
}
