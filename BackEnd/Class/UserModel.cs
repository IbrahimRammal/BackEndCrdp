namespace BackEnd.Class
{
    public class UserModel
    {
        public int Id { get; set; }
        public int? Key { get; set; }
        public string? UserName { get; set; }
        public UserType UserType { get; set; }
        public string? FullName { get; set; }
        public bool EmailConfirmed { get; set; }

    }
}
