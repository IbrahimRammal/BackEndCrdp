using BackEnd.Class;

namespace BackEnd.Identity
{
    public interface IUserManager
    {
        Task<TokenModel> AuthenticateAsync(string username, string password);
        Task<UserModel> Validate(string username, string password, bool hased = false);
       TokenModel GenerateJwtToken(UserModel user);
        Task<UserModel> GetByIdAsync(int id);
    }
}
