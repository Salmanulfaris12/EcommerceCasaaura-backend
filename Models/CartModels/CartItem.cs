using CasaAura.Models.ProductModels;
using CasaAura.Models.UserModels;
using System.Text.Json.Serialization;

namespace CasaAura.Models.CartModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId {  get; set; }
        public int Quantity {  get; set; }
        public virtual Cart ? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
