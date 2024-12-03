namespace CasaAura.Models.WishListModels.WishListDTOs
{
    public class WishListResDTO
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? Material {  get; set; }
        public decimal ? Price {  get; set; }
        public string ?Category {  get; set; }
        public string ?Image {  get; set; }
    }
}
