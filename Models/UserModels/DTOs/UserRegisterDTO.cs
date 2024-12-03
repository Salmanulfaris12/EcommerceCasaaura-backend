using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.UserModels.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]

        public string Password { get; set; }

    }
}
