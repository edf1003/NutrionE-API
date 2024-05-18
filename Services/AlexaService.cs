using Microsoft.EntityFrameworkCore;
using NutrionE.Models;
using NutrionE.Services.Interfaces;
using System.Globalization;
namespace NutrionE.Services
{
    public class AlexaService (NutrioneContext context, IChatGPTService chatGPTService, IUsersService usersService) : IAlexaService
    {
        private readonly NutrioneContext context = context;
        private readonly IChatGPTService chatGPTService = chatGPTService;
        private readonly IUsersService usersService = usersService;

        #region Diet
        public async Task<string> GetDailyDiet(string userId)
        {
            await CheckUserExistsAsync(userId);
            var date = DateTimeOffset.Now.Date;
            var dbItem = await context.DailyDiets.Include(d => d.Meals)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == date);
            
            if (dbItem == null)
            {
                return await chatGPTService.CreateDailyDiet(userId);
            }

            return $"¡Hola! Aquí está tu dieta para hoy: " +
                         $"Desayuno: {dbItem.Meals[0].FirstPlate}. "+
                         $"Comida: {dbItem.Meals[1].FirstPlate}, {dbItem.Meals[1].SecondPlate}, {dbItem.Meals[1].Dessert}. " +
                         $"Cena: {dbItem.Meals[2].FirstPlate}, {dbItem.Meals[2].SecondPlate}, {dbItem.Meals[2].Dessert}. " +
                         $"¡Que disfrutes tu comida!";
        }

        public async Task<string> GetDailyBreakfast(string userID)
        {
            var date = DateTimeOffset.Now.Date;
            var dbItem = await context.DailyDiets.Include(d => d.Meals)
                .FirstOrDefaultAsync(d => d.UserId == userID && d.Date == date);

            if (dbItem == null)
            {
                return "Tienes que crear una dieta para hoy antes de consultarla";
            }

            return $"¡Hola! Aquí está tu desayuno para hoy: {dbItem.Meals[0].FirstPlate}.";
        }

        public async Task<string> GetDailyLunch(string userId)
        {
            var date = DateTimeOffset.Now.Date;
            var dbItem = await context.DailyDiets.Include(d => d.Meals)
                .Where(d => d.Meals.Any(e => e.Name == "Lunch"))
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == date);

            if (dbItem == null)
            {
                return "Tienes que crear una dieta para hoy antes de consultarla";
            }

            return $"¡Hola! Aquí está tu comida para hoy:\n\n" +
                        $"Plato principal - {dbItem.Meals[1].FirstPlate}, " +
                        $"Segundo plato - {dbItem.Meals[1].SecondPlate}, " +
                        $"Postre - {dbItem.Meals[1].Dessert}.";
        }   

        public async Task<string> GetDailyDinner(string userId)
        {
            var date = DateTimeOffset.Now.Date;
            var dbItem = await context.DailyDiets.Include(d => d.Meals)
                .Where(d => d.Meals.Any(e => e.Name == "Dinner"))
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == date);

            if (dbItem == null)
            {
                return "Tienes que crear una dieta para hoy antes de consultarla";
            }

            return $"¡Hola! Aquí está tu cena para hoy:\n\n" +
                        $"Plato principal - {dbItem.Meals[2].FirstPlate}, " +
                        $"Segundo plato - {dbItem.Meals[2].SecondPlate}, " +
                        $"Postre - {dbItem.Meals[2].Dessert}.";
        }


        #endregion Diet

        #region Routine
        public async Task<string> GetDailyRoutine(string userId, string exercise)
        {
            await CheckUserExistsAsync(userId);
            var date = DateTimeOffset.Now.Date;
            var exerciseType = ExerciseType.Arm;
            switch (exercise)
            {
                case "brazo":
                    exerciseType = ExerciseType.Arm;
                    break;
                case "pierna":
                    exerciseType = ExerciseType.Leg;
                    break;
                case "espalda":
                    exerciseType = ExerciseType.Back;
                    break;
                case "pecho":
                    exerciseType = ExerciseType.Chest;
                    break;
                case "cardio":
                    exerciseType = ExerciseType.Cardio;
                    break;
            }
             var dbItem = await context.DailyRoutines.Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.UserId == userId && d.Date == date && d.ExerciseType == exerciseType);

            if (dbItem == null)
            {
                return await chatGPTService.CreateDailyRoutine(userId, exerciseType);
            }

            string dietDescription = $"¡Hola! Aquí están tus ejercicios para tu rutina de {dbItem.ExerciseType} de hoy:" +
                $"1. {dbItem.Exercises[0].Name}." +
                $"2. {dbItem.Exercises[1].Name}" +
                $"3. {dbItem.Exercises[2].Name}." +
                $"4. {dbItem.Exercises[3].Name}." +
                $"5. {dbItem.Exercises[4].Name}." +
                $"6. {dbItem.Exercises[5].Name}." +
                $"¡Que disfrutes tu entrenamiento!";
            return dietDescription;
        }

        #endregion Routine

        private async Task<User?> CheckUserExistsAsync(string userId)
        {
            var user = await usersService.GetUserByUserName(userId);
            if (user == null)
            {
                await usersService.CreateUser(userId);
            }
            return user;
        }
    }
}
