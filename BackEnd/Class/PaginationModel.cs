using Microsoft.AspNetCore.Identity;

namespace BackEnd.Class
{
    internal class PaginationModel
    {
        public List<IdentityUser> users { get; set; }
        public int totalCount { get; set; }
    }
}