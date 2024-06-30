using NutrionE.Models;
using NutrionE.Models.DTOs.User;

namespace NutrionE.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUserByUserName(string userId);

        Task<OperationResult<User>> GetUser(string userId);

        Task<User> CreateUser(string userId);

        Task<User?> UpdateUser(string userId, DietType dietType);

        Task<User?> UpdateUser(UserDTO user);

        Task DeleteUser(string userId);

        Task<string> GetUserDietType(string userId);

        Task<string> GetUserGender(string userId);

        Task<string> GetUserWeight(string userId);

        Task<string> GetUserHeight(string userId);

        Task<string> GetUserAge(string userId);

        Task<string> GetUserName(string userId);

        Task<string> GetUserMail(string userId);

        Task UpdateUserName(string userId, string name);

        Task UpdateUserAge(string userId, string age);

        Task UpdateUserWeight(string userId, string weight);

        Task UpdateUserHeight(string userId, string height);

        Task UpdateUserGender(string userId, string gender);

        Task UpdateUserDietType(string userId, string dietType);

        Task UpdateUserObjective(string userId, string objective);

        Task UpdateUserMail(string userId, string mail);
    }
}
