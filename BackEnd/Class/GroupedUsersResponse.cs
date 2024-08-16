namespace BackEnd.Class
{
    public class GroupedUsersResponse
    {
        public int WorkGroupCount { get; set; }
        public List<GroupedUsersDto> GroupedUsers { get; set; }
    }
}
