using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using NutrionE.Models;
using NutrionE.Services.Interfaces;

namespace NutrionE.Services
{
    public class UsersService (NutrioneContext context) : IUsersService
    {

        private readonly NutrioneContext context = context;

        public async Task<User> GetUserByUserName(string userId)
        {
            var dbItem = await context.Users.Where(e => e.Id == userId).FirstOrDefaultAsync();
            if (dbItem == null) return null;
            return dbItem;
        }

        public async Task<User> CreateUser(string userId)
        {
            var user = new User
            {
                Id = userId,
                EnrollmentDate = DateTimeOffset.Now,
                DietType = DietType.Omnivore,
                Age = 0,
                Gender = Gender.Male,
                Height = 0,
                Weight = 0,
                Name = "",
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(string userId, DietType dietType)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return null;

            dbItem.DietType = dietType;
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();
            return dbItem;
        }

        public async Task DeleteUser(string userId)
        {
            context.Users.Remove(await GetUserByUserName(userId));
            await context.SaveChangesAsync();
        }


        #region GetUserData

        public async Task<string> GetUserDietType(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.DietType.ToString();
        }

        public async Task<string> GetUserName(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.Name;
        }

        public async Task<string> GetUserAge(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.Age.ToString();
        }

        public async Task<string> GetUserWeight(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.Weight.ToString() + "Kg";
        }

        public async Task<string> GetUserHeight(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.Height.ToString() + "cm";
        }

        public async Task<string> GetUserGender(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";

            return dbItem.Gender == Gender.Male ? "Hombre" : "Mujer";
        }

        public async Task<string> GetUserObjective(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.DietObjective.ToString();
        }

        public async Task<string> GetUserMetabolism(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.BasalMetabolism.ToString();
        }

        public async Task<string> GetUserMail(string userId)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return "";
            return dbItem.Email;
        }

        #endregion UserData


        #region UpdateUserData

        public async Task UpdateUserName(string userId, string name)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null)
            {
                await CreateUser(userId);
                dbItem = await GetUserByUserName(userId);
            }
            dbItem.Name = name;
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserAge(string userId, string age)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            if (int.TryParse(age, out int parsedAge))
            {
                dbItem.Age = parsedAge;
            }
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();

            await UpdateUserMetabolism(userId);
        }

        public async Task UpdateUserWeight(string userId, string weight)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            if (int.TryParse(weight, out int parsedWeight))
            {
                dbItem.Weight = parsedWeight;
            }
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();

            await UpdateUserMetabolism(userId);
        }

        public async Task UpdateUserHeight(string userId, string height)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            if (int.TryParse(height, out int parsedHeight))
            {
                dbItem.Height = parsedHeight;
            }
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();

            await UpdateUserMetabolism(userId);
        }

        public async Task UpdateUserGender(string userId, string gender)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            dbItem.Gender = gender == "hombre" ? Gender.Male : Gender.Female;
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();

            await UpdateUserMetabolism(userId);
        }

        public async Task UpdateUserDietType(string userId, string dietType)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            Dictionary<string, DietType> dietTypeMapping = new ()
            {
                { "carnívora", DietType.Carnivore },
                { "carnivora", DietType.Carnivore },
                { "omnívora", DietType.Omnivore },
                { "omnivora", DietType.Omnivore },
                { "paleo", DietType.Paleo },
                { "vegana", DietType.Vegan },
                { "vegetariana", DietType.Vegetarian },
                { "mediterránea", DietType.Mediterranean },
                { "mediterranea", DietType.Mediterranean }
            };

            if (dietTypeMapping.TryGetValue(dietType, out DietType value))
            {
                dbItem.DietType = value;
            }
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserObjective(string userId, string objective)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            Dictionary<string, DietObjective> objectives = new()
            {
                { "ganar peso", DietObjective.GainWeight },
                { "aumentar peso", DietObjective.GainWeight },
                { "ganar algo de peso", DietObjective.GainWeight },
                { "ganar", DietObjective.GainWeight },
                { "engordar", DietObjective.GainWeight },
                { "perder peso", DietObjective.LoseWeight },
                { "disminuir pero", DietObjective.LoseWeight },
                { "perder algo de peso", DietObjective.LoseWeight },
                { "adelgazar", DietObjective.LoseWeight },
                { "perder", DietObjective.LoseWeight },
                { "cuidar mi peso", DietObjective.MaintainWeight },
                { "mantener mi peso", DietObjective.MaintainWeight },
                { "continuar en mi peso", DietObjective.MaintainWeight },
                { "mantener peso", DietObjective.MaintainWeight },
                { "mantenerme", DietObjective.MaintainWeight },
            };

            if (objectives.TryGetValue(objective, out DietObjective value))
            {
                dbItem.DietObjective = value;
            }
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserMetabolism(string userId)
        {
            var user = await GetUserByUserName(userId);
            if (user == null) return;
            if (user.Height == 0 || user.Weight == 0 || user.Age == 0) return;
            var multiplicationFactor = user.Gender == Gender.Male ? 5 : -161;
            user.BasalMetabolism = (decimal)((10 * user.Weight) + ((decimal)6.25 * user.Height) - (5 * user.Age) + multiplicationFactor);
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUserMail(string userId, string mail)
        {
            var dbItem = await GetUserByUserName(userId);
            if (dbItem == null) return;

            dbItem.Email = mail;
            context.Users.Update(dbItem);
            await context.SaveChangesAsync();
        }

        #endregion UpdateUserData
    }
}
