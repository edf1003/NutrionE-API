using Microsoft.EntityFrameworkCore;
using NutrionE.Configuration;
using NutrionE.Models;
using NutrionE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NutrioneContext>(options => options.UseSqlServer("Server=KIKE\\SQLDEV;Database=NutrionE;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddHttpClient<GoogleFitService>(); 
ConfigureServiceDeclarations.ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().
    AllowAnyMethod().
    AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
