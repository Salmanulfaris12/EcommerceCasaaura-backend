using CasaAura.Models.AddressModels;
using CasaAura.Models.UserModels;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;

namespace CasaAura.Models.OrderModels
{
    public class OrderMain
    {
        public int Id { get; set; }
        [Required]
        public int UserId {  get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public string OrderString { get; set; }
        [Required]
        public string TransactionId { get; set; }

        public virtual User? User { get; set; }
        public virtual Address? Address { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }


        


    }
}
