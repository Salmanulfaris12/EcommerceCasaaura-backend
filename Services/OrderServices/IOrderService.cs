﻿using CasaAura.Models.OrderModels.OrderDTOs;

namespace CasaAura.Services.OrderServices
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(PaymentDTO payment);
        Task<bool>CreateOrder(int userId ,CreateOrderDTO createOrderDTO);
        Task UpdateOrder(string orderId, string orderStatus);
        Task<List<ViewOrderAdminDTO>> GetOrderDetailsAdmin();
        Task<List<ViewOrderUserDetailDTO>>GetOrderDetails(int userId);
        Task<List<ViewOrderUserDetailDTO>> GetOrdersByUserId(int userId);
        Task<decimal> TotalRevenue();
        Task<int> TotalProductsPurchased();
    }
}
