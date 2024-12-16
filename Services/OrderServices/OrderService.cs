using CasaAura.Models.CartModels.CartDTOs;
using CasaAura.Models.OrderModels;
using CasaAura.Models.OrderModels.OrderDTOs;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace CasaAura.Services.OrderServices
{
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public OrderService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> RazorOrderCreate(long price)
        {
            Dictionary<string,object>input= new Dictionary<string,object>();
            Random random= new Random();
            string TransactionId=random.Next(0,1000).ToString();

            input.Add("amount",Convert.ToDecimal(price)*100);
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client= new RazorpayClient(key, secret);
            Razorpay.Api.Order order=client.Order.Create(input);
            var OrderId = order["id"].ToString();
            return OrderId;
        }
        public bool RazorPayment(PaymentDTO payment)
        {
            if(payment==null ||
                string.IsNullOrEmpty(payment.razorpay_payment_id)||
                string.IsNullOrEmpty(payment.razorpay_orderId)||
                string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return false;
            }
            try
            {
                RazorpayClient client = new RazorpayClient(
                    _configuration["Razorpay:KeyId"],
                    _configuration["Razorpay:Keysecret"]
                    );
                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", payment.razorpay_payment_id },
                    { "razorpay_order_id", payment.razorpay_orderId },
                    { "razorpay_signature", payment.razorpay_signature }
                };
                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDTO)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefaultAsync(x => x.UserId == userId);
                if (cart == null)
                {
                    throw new Exception("cart is empty");
                }
                var order = new OrderMain
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatus = "pending",
                    AddressId = createOrderDTO.AddressId,
                    TotalAmount = createOrderDTO.Totalamount,
                    OrderString = createOrderDTO.OrderString,
                    TransactionId = createOrderDTO.TransactionId,
                    OrderItems = cart.CartItems.Select(c => new OrderItem
                    {
                        productId = c.ProductId,
                        Quantity = c.Quantity,
                        TotalPrice = c.Quantity * c.Product.ProductPrice
                    }).ToList(),
                };
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
                    if (product != null)
                        if (product.Stock < cartItem.Quantity)
                        {
                            return false;
                        }

                    product.Stock -= cartItem.Quantity;

                }
                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ViewOrderUserDetailDTO>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _context.Orders.Include(i => i.OrderItems)
                                .ThenInclude(i => i.Product)
                                .Where(i => i.UserId == userId)
                                .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return new List<ViewOrderUserDetailDTO>();
                }
                var orderdetails = orders.Select(i => new ViewOrderUserDetailDTO
                {
                    Id = i.Id,
                    OrderId = i.OrderString,
                    OrderDate = i.OrderDate,
                    OrderStatus = i.OrderStatus,
                    TransactionId = i.TransactionId,
                    OrderProducts = i.OrderItems.Select(o => new CartViewDTO
                    {
                        ProductId = o.productId,
                        ProductName = o.Product.ProductName,
                        Price = o.Product.ProductPrice,
                        TotalAmount = o.TotalPrice,
                        Quantity = o.Quantity,
                        Image = o.Product.Image
                    }).ToList(),

                }).ToList();
                return orderdetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<ViewOrderAdminDTO>> GetOrderDetailsAdmin()
        {
            var orderdetails = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Select(o => new ViewOrderAdminDTO
                {
                    id = o.Id,
                    OrderDate = o.OrderDate,
                    orderId=o.OrderString,
                    CustomerEmail = o.User.Email,
                    OrderStatus=o.OrderStatus,
                    CustomerName = o.User.Name,
                    TransactionId = o.TransactionId
                })
                .ToListAsync();

            return orderdetails;
            //return new List<ViewOrderAdminDTO>();
        }
        public async Task<decimal> TotalRevenue()
        {
            try
            {
              var total= await _context.OrderItems.SumAsync(p=>p.TotalPrice);
                return total;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int>TotalProductsPurchased()
        {
            try
            {
                var totalproduct=await _context.OrderItems.SumAsync(p=>p.Quantity);
                return totalproduct;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ViewOrderUserDetailDTO>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders=await _context.Orders.Include(i=>i.OrderItems)
                                .ThenInclude(i=>i.Product)
                                .Where(i=>i.UserId == userId)
                                .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return new List<ViewOrderUserDetailDTO>();
                }
                var orderdetails = orders.Select(i => new ViewOrderUserDetailDTO
                {
                    Id = i.Id,
                    OrderId = i.OrderString,
                    OrderDate = i.OrderDate,
                    OrderStatus=i.OrderStatus,
                    TransactionId = i.TransactionId,
                    OrderProducts = i.OrderItems.Select(o => new CartViewDTO
                    {
                        ProductId = o.productId,
                        ProductName = o.Product.ProductName,
                        Price = o.Product.ProductPrice,
                        TotalAmount = o.TotalPrice,
                        Quantity = o.Quantity,
                        Image = o.Product.Image
                    }).ToList(),

                }).ToList();
                return orderdetails;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateOrder(string orderId, string orderStatus)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderString == orderId);
                if (order != null)
                {
                    order.OrderStatus = orderStatus;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Order with this order Id not found");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
