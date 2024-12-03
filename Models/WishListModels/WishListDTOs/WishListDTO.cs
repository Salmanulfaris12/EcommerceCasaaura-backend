using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.WishListModels.WishListDTOs
{
    public class WishListDTO
    {
        [Required]
        public int UserId {  get; set; }
        [Required]
        public int ProductId {  get; set; }
    }
}
