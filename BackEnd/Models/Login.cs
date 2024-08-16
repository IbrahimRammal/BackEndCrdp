using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Login
    {
        [Key]
        public  Guid UId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
