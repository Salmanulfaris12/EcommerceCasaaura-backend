namespace CasaAura.Models.CartModels.CartDTOs
{
    public class CartResDTO
    {
        public List<CartViewDTO> cartItemsperUser { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
