namespace CasaAura.Models.OrderModels.OrderDTOs
{
    public class PaymentDTO
    {
        public string ?razorpay_payment_id {  get; set; }
        public string ?razorpay_orderId { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
