namespace CasaAura.Models.OrderModels.OrderDTOs
{
    public class ViewOrderAdminDTO
    {
        public int id {  get; set; }
        public string orderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId {  get; set; }
    }
}
