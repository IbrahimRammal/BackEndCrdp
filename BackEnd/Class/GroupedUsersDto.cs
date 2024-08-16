namespace BackEnd.Class
{
    public class GroupedUsersDto
    {
        public string? GroupName { get; set; }
        public List<UserDto>? Users { get; set; }
        public int? UserCount => Users?.Count;
    }
}