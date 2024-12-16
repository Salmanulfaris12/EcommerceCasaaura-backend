namespace CasaAura.Models.OrderModels.OrderDTOs
{
    public class ViewOrderDTO
    {
        public int Id { get; set; }
        public string ProductName {  get; set; }
        public string Image {  get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderId { get; set; }

    }
}
