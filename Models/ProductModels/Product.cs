using CasaAura.Models.CartModels;
using CasaAura.Models.CategoryModels;
using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.ProductModels
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Product name is required")]
        public string? ProductName { get; set; }
        [Required(ErrorMessage ="Production description is required")]
        public string? ProductDescription { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal ProductPrice { get; set; }
        //[Required(ErrorMessage ="Image url is required")]
        //[Url(ErrorMessage ="Invalid url Format")]
        public string? Image {  get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string? Material {  get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal 0")]
        public int Stock {  get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal MRP { get; set; }
        public virtual Category? Category { get; set; }
        public virtual List<CartItem> CartItems { get; set; }

    }
}
