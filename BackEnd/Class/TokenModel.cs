using BackEnd.Models;

namespace BackEnd.Class
{
    public class TokenModel
    {
        public int UserId { get; set; }
        public int KeyId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public UserType UserType { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public DateTime ExpiresOn { get; set; }

    }
}
