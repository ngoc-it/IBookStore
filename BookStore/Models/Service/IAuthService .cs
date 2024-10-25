using BookStore.Models.Data;
using BookStore.Models.Model;

namespace BookStore.Models.Service
{
    public interface IAuthService
    {
        Task InsertUser(RegisterModel model);
        Task<User> AuthenticationUser(UserModel model);

    }
}
