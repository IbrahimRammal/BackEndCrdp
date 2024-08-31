using BackEnd.Class;
using BackEnd.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Identity
{
    public class UserManager : IUserManager
    {
        private readonly CrdpCurriculumMsContext _context;
        private readonly AppSettings _settings;
        public UserManager(CrdpCurriculumMsContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }
        public async Task<TokenModel> AuthenticateAsync(string username, string password)
        {
            var result = await Validate(username, password);
            if (result != null)
            {
                return GenerateJwtToken(result);
            }
            return null;
        }

        public TokenModel GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(6650), // Long expiration period, consider revising
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenKey = tokenHandler.WriteToken(token);
            return new TokenModel
            {
                UserId = user.Id,
                KeyId = user.Key ?? 0,
                UserName = user.UserName,
                FullName = user.FullName,
                UserType = user.UserType,
                ExpiresOn = tokenDescriptor.Expires.Value.ToLocalTime(),
                Token = tokenKey
            };
        }

        public Task<UserModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> Validate(string username, string password, bool hased = false)
        {
            throw new NotImplementedException();
        }
    }
}
