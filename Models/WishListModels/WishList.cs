using CasaAura.Models.ProductModels;
using CasaAura.Models.UserModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.WishListModels
{
    public class WishList
    {
        [Required]
        public int Id {  get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId {  get; set; }
        public virtual User? Users { get; set; }
        public virtual Product? Products { get; set; }
    }
}
