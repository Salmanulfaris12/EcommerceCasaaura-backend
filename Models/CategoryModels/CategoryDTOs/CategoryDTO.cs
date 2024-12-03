using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.CategoryModels.CategoryDTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }
    }
}
