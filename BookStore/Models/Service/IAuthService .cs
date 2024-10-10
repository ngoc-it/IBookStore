using BookStore.Models.Data;
using BookStore.Models.Model;

namespace BookStore.Models.Service
{
    public interface IAuthService
    {
        Task InsertUser(RegisterModel model);
        Task<User> AuthenticationUser(UserModel model);
        //Task<string> HashPassword(string value);
        //Task<bool> ValidateHashPassword(string value, string hash);
    }
}
