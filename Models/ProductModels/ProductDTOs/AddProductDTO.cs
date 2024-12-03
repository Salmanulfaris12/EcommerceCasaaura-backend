using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.ProductModels.ProductDTOs
{
    public class AddProductDTO
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Material {  get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal ProductPrice {  get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal MRP { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int Stock { get; set; }  

    }
}
