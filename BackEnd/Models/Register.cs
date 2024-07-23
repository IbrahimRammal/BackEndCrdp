using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Register
    {
        [Key]
        public virtual Guid Id { get; set; }
        public string UserName { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
    }
}
