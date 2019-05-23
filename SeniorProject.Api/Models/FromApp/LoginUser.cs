using System.ComponentModel.DataAnnotations;

namespace SeniorProject.Api.Models.FromApp
{
    public class LoginUser
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
