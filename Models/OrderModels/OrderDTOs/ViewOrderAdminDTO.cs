namespace CasaAura.Models.OrderModels.OrderDTOs
{
    public class ViewOrderAdminDTO
    {
        public int id {  get; set; }
        public int orderId { get; set; }
        public string CustomerNmae { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId {  get; set; }
    }
}
