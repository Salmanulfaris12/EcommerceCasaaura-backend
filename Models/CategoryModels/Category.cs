using CasaAura.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.CategoryModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        public virtual ICollection<Product>?Products { get; set; }
    }
}
