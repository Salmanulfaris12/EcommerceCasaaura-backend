namespace CasaAura.Models.UserModels.DTOs
{
    public class UserViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }

    }
}
