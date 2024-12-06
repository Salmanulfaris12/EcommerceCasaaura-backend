using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.CategoryModels.CategoryDTOs
{
    public class CategoryDTO
    {
       
        [Required]
        public string? CategoryName { get; set; }
    }
}
