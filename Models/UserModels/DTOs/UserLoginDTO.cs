using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.UserModels.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
