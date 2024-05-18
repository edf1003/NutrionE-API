using NutrionE.Models;

namespace NutrionE.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<User> GetUserByUserName(string userId);

        public Task<User> CreateUser(string userId);

        public Task<User?> UpdateUser(string userId, DietType dietType);

        public Task DeleteUser(string userId);

        public Task<string> GetUserDietType(string userId);

        public Task<string> GetUserGender(string userId);

        public Task<string> GetUserWeight(string userId);

        public Task<string> GetUserHeight(string userId);

        public Task<string> GetUserAge(string userId);

        public Task<string> GetUserName(string userId);

        public Task<string> GetUserMail(string userId);

        public Task UpdateUserName(string userId, string name);

        public Task UpdateUserAge(string userId, string age);

        public Task UpdateUserWeight(string userId, string weight);

        public Task UpdateUserHeight(string userId, string height);

        public Task UpdateUserGender(string userId, string gender);

        public Task UpdateUserDietType(string userId, string dietType);

        public Task UpdateUserObjective(string userId, string objective);

        public Task UpdateUserMail(string userId, string mail);
    }
}
