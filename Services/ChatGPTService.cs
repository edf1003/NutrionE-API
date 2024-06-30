using NutrionE.Models;
using NutrionE.Services.Interfaces;
using NutrionE.Models.DTOs.ChatGptResponsesDTO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using System;

namespace NutrionE.Services
{
    public class ChatGPTService (NutrioneContext context) : IChatGPTService
    {
        private readonly NutrioneContext context = context;

        private static async Task<string> GetPetition(string prompt)
        {
            string baseUrl = "https://api.openai.com/v1/completions";
            string apiKey = "";
            string jsonParams = $"{{\"model\": \"gpt-3.5-turbo-instruct\", \"prompt\": \"{prompt}\", \"max_tokens\": 500}}";

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            HttpResponseMessage response = await client.PostAsync($"{baseUrl}", new StringContent(jsonParams, null, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                MainResponse? mealResponse = JsonConvert.DeserializeObject<MainResponse>(jsonResponse, new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                });
                return mealResponse.Choices[0].Text;
            }
            return "";
        }


        private async Task<User> GetUser(string userId)
        {
            return await context.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
        }


        #region Diets
        public async Task<string> CreateDailyDiet(string userId)
        {
            var user = await GetUser(userId);
            if (user == null)
                return "Ha ocurrido un error al procesar la solicitud, por favor intenta de nuevo.";

            List<Meal> meals = new();
            string genderPrefix = GetGenderPrefix(user);
            string diet = GetDietType(user);
            string objective = GetDietObjective(user);

            string prompt = GenerateDietPrompt(genderPrefix, user, objective, diet);
            string mealText = await GetProcessedMealText(prompt);
            string[] plates = mealText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (plates.Length == 7)
            {
                var dailyDiet = await CreateDailyDietRecord(userId);
                meals.AddRange(CreateMeals(dailyDiet.Id, plates));
                await context.Meals.AddRangeAsync(meals);
                await context.SaveChangesAsync();

                return GenerateDietDescription(meals);
            }
            else
                return await CreateDailyDiet(userId);
        }

        private string GetGenderPrefix(User user)
        {
            return user?.Gender == Gender.Female ? "una mujer" : "un hombre";
        }

        private static string GetDietType(User user)
        {
            var dietTypeMapping = new Dictionary<DietType, string>
            {
                { DietType.Carnivore, "carnívora" },
                { DietType.Omnivore , "omnívora" },
                { DietType.Paleo , "paleo" },
                { DietType.Vegan , "vegana" },
                { DietType.Vegetarian , "vegetariana" },
                { DietType.Mediterranean , "mediterranea" }
            };
            return dietTypeMapping.TryGetValue(user.DietType, out string? dietType) ? dietType : "omnívora";
        }

        private static string GetDietObjective(User user)
        {
            var dietObjectiveMapping = new Dictionary<DietObjective, string>
            {
                { DietObjective.GainWeight, "Subir de peso" },
                { DietObjective.LoseWeight, "Bajar de peso" },
                { DietObjective.MaintainWeight, "Mantener el peso" }
            };
            return dietObjectiveMapping.TryGetValue(user.DietObjective, out string? dietObjective) ? dietObjective : "Subir de peso";
        }

        private static string GenerateDietPrompt(string genderPrefix, User user, string objective, string diet)
        {
            return $"Genera una lista no numerada de 7 platos con sus cantidades para, " +
                   $"un desayuno de un único plato, una comida con primer plato, segundo plato y postre " +
                   $"y una cena con primer plato, segundo plato y postre. Indicando solo los nombres de los platos con sus cantidades, sin ningún dato más" +
                   $"Para una persona de género {genderPrefix}, que mide {user?.Height} centímetros, pesa {user?.Weight} kilogramos, " +
                   $"tiene un metabolismo basal de {user?.BasalMetabolism} y cuyo objetivo es {objective}. " +
                   $"La dieta debe ser {diet}.";
        }

        private static async Task<string> GetProcessedMealText(string prompt)
        {
            string mealText = await GetPetition(prompt);
            mealText = mealText.Trim();
            mealText = mealText.Replace("\n", ",");
            string mealPattern = @"Desayuno:|Comida:|Cena:|Entrante:|Primer plato:|Segundo plato:|Postre:|Plato principal:";
            mealText = Regex.Replace(mealText, mealPattern, "", RegexOptions.IgnoreCase);
            string punctuationPattern = @"[.;!?-]";
            mealText = Regex.Replace(mealText, punctuationPattern, ",");
            /*string numberPattern = @"\d+";
            mealText = Regex.Replace(mealText, numberPattern, "");*/
            string spacesPattern = @",\s*,";
            mealText = Regex.Replace(mealText, spacesPattern, ",");
            mealText = Regex.Replace(mealText, spacesPattern, ",");
            return mealText;
        }

        private async Task<DailyDiet> CreateDailyDietRecord(string userId)
        {
            var dailyDiet = new DailyDiet
            {
                UserId = userId,
                Date = DateTimeOffset.Now.Date,
                DietType = DietType.Mediterranean
            };

            await context.DailyDiets.AddAsync(dailyDiet);
            await context.SaveChangesAsync();
            return dailyDiet;
        }

        private List<Meal> CreateMeals(long dailyDietId, string[] plates)
        {
            List<Meal> meals = [];

            meals.Add(new Meal
            {
                FirstPlate = plates[0].Trim(),
                SecondPlate = "",
                Dessert = "",
                DailyDietId = dailyDietId,
                Name = "Breakfast"
            });

            meals.Add(new Meal
            {
                FirstPlate = plates[1].Trim(),
                SecondPlate = plates[2].Trim(),
                Dessert = plates[3].Trim(),
                DailyDietId = dailyDietId,
                Name = "Lunch"
            });

            meals.Add(new Meal
            {
                FirstPlate = plates[4].Trim(),
                SecondPlate = plates[5].Trim(),
                Dessert = plates[6].Trim(),
                DailyDietId = dailyDietId,
                Name = "Dinner"
            });

            return meals;
        }

        private static string GenerateDietDescription(List<Meal> meals)
        {
            return $"¡Hola! Aquí está tu dieta para hoy: " +
                   $"Desayuno: {meals[0].FirstPlate}. " +
                   $"Comida: {meals[1].FirstPlate}, {meals[1].SecondPlate}, {meals[1].Dessert}. " +
                   $"Cena: {meals[2].FirstPlate}, {meals[2].SecondPlate}, {meals[2].Dessert}. " +
                   $"¡Que disfrutes tu comida!";
        }

        #endregion Diets

        #region Routines

        public async Task<string> CreateDailyRoutine(string userId, ExerciseType exerciseType)
        {
            var user = await GetUser(userId);
            if (user == null)
                return "Ha ocurrido un error al procesar la solicitud, por favor intenta de nuevo.";

            string genderPrefix = GetGenderPrefix(user);
            string prompt = GenerateRoutinePrompt(genderPrefix, user, exerciseType);
            string exerciseText = await GetProcessedExerciseText(prompt);
            string[] exercises = exerciseText.Split(',');

            if (exercises.Length == 6)
            {
                var dailyRoutine = await CreateDailyRoutineRecord(userId, exerciseType);
                List<Exercise> exerciseList = CreateExercises(dailyRoutine.Id, exercises);
                await context.Exercises.AddRangeAsync(exerciseList);
                await context.SaveChangesAsync();

                return GenerateRoutineDescription(exerciseType, exerciseList);
            }
            else
                return await CreateDailyRoutine(userId, exerciseType);
        }

        private static string GenerateRoutinePrompt(string genderPrefix, User user, ExerciseType exerciseType)
        {
            return $"Genera una rutina de 6 ejercicios de gimnasio para ejercicios de {GetExerciseTypeName(exerciseType)}. Indicame solo el nombre de los ejercicios y sus cantidades separados por comas." +
                   $"Para {genderPrefix} persona que mide {user?.Height} centimetos y pesa {user?.Weight} kilogramos.";
        }

        private static async Task<string> GetProcessedExerciseText(string prompt)
        {
            string exerciseText = await GetPetition(prompt);
            exerciseText = exerciseText.TrimStart('\n').Replace("\n", ",");
            exerciseText = exerciseText.Replace("Ejercicio 1", "").Replace("Ejercicio 2", "").Replace("Ejercicio 3", "").Replace("Ejercicio 4", "").Replace("Ejercicio 5", "").Replace("Ejercicio 6", "");
            exerciseText = exerciseText.Replace("1. ", "").Replace("2. ", "").Replace("3. ", "").Replace("4. ", "").Replace("5. ", "").Replace("6. ", "");
            exerciseText = exerciseText.Replace(":", "").Replace(";", "").Replace(".", "").Replace("!", "").Replace("?", "").Replace("-", "");
            exerciseText = exerciseText.Replace(", , ,", ",").Replace(",,", ",");
            return exerciseText;
        }

        private async Task<DailyRoutine> CreateDailyRoutineRecord(string userId, ExerciseType exerciseType)
        {
            var dailyRoutine = new DailyRoutine
            {
                UserId = userId,
                Date = DateTimeOffset.Now.Date,
                ExerciseType = exerciseType
            };

            await context.DailyRoutines.AddAsync(dailyRoutine);
            await context.SaveChangesAsync();
            return dailyRoutine;
        }

        private List<Exercise> CreateExercises(long dailyRoutineId, string[] exercises)
        {
            List<Exercise> exerciseList = [];

            for (int i = 0; i < 6; i++)
            {
                exerciseList.Add(new Exercise
                {
                    Name = exercises[i].Trim(),
                    DailyRoutineId = dailyRoutineId
                });
            }

            return exerciseList;
        }

        private static string GenerateRoutineDescription(ExerciseType exerciseType, List<Exercise> exerciseList)
        {
            return $"¡Hola! Aquí está tu rutina de ejercicios de {GetExerciseTypeName(exerciseType)} para hoy:\n\n" +
                   $"Ejercicio 1: {exerciseList[0].Name}. \n" +
                   $"Ejercicio 2: {exerciseList[1].Name}. \n" +
                   $"Ejercicio 3: {exerciseList[2].Name}. \n" +
                   $"Ejercicio 4: {exerciseList[3].Name}. \n" +
                   $"Ejercicio 5: {exerciseList[4].Name}. \n" +
                   $"Ejercicio 6: {exerciseList[5].Name}. \n\n" +
                   $"¡A darle duro!";
        }
        
        private static string GetExerciseTypeName(ExerciseType exerciseType)
        {
            return exerciseType switch
            {
                ExerciseType.Cardio => "Cardio",
                ExerciseType.Leg => "Piernas",
                ExerciseType.Arm => "Brazos",
                ExerciseType.Chest => "Pecho",
                ExerciseType.Back => "Espalda",
                ExerciseType.Gluteus => "Glúteos",
                _ => "Cardio",
            };
        }

        #endregion Routines
    }
}