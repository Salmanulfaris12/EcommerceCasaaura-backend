namespace CasaAura.Models.ProductModels.ProductDTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }
        public string Image {  get; set; }
        public string Category { get; set; }
        public string Material {  get; set; }
        public decimal MRP { get; set; }
        public int Stock { get; set; }
    }
}
