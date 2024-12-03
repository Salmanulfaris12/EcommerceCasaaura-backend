using CasaAura.Models.UserModels;
using System.Text.Json.Serialization;

namespace CasaAura.Models.CartModels
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
    }
}
