using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.AddressModels.AddresDTOs
{
    public class AddressCreateDTO
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(20, ErrorMessage = "Full name must not exceed 20 characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits")]
        public string? Pincode { get; set; }

        [Required(ErrorMessage = "House name is required")]
        [StringLength(20, ErrorMessage = "House name must not exceed 20 characters")]
        public string? HouseName { get; set; }

        [Required(ErrorMessage = "Place is required")]
        [StringLength(20, ErrorMessage = "Place must not exceed 20 characters")]
        public string? Place { get; set; }

        [Required(ErrorMessage = "Post office is required")]
        [StringLength(20, ErrorMessage = "Post office must not exceed 20 characters")]
        public string? PostOffice { get; set; }

        [Required(ErrorMessage = "Land mark is required")]
        [StringLength(200, ErrorMessage = "Landmark must not exceed 200 characters")]
        public string? LandMark { get; set; }

    }
}
