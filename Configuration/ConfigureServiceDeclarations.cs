using NutrionE.Services;
using NutrionE.Services.Interfaces;

namespace NutrionE.Configuration
{
    public static class ConfigureServiceDeclarations
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            //Register services here
            services.AddScoped<IAlexaService, AlexaService>();
            services.AddScoped<IChatGPTService, ChatGPTService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGoogleFitService, GoogleFitService>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddSingleton(configuration);
            services.AddHttpClient("Auth", x => { x.Timeout = TimeSpan.FromMinutes(10); });
        }

    }
}
