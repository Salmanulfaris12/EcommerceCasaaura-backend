namespace CasaAura.Models.CartModels.CartDTOs
{
    public class CartViewDTO
    {
        public int ProductId {  get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image {  get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity {  get; set; }
    }
}
