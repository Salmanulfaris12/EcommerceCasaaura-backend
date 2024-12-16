using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.OrderModels.OrderDTOs;
using CasaAura.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }
        [HttpPost("order-create")]
        [Authorize]
        public async Task<IActionResult>CreateOrder(long price)
        {
            try
            {
                if (price <= 0)

                {
                    return BadRequest("enter valid price");
                }
                var orderId = await _service.RazorOrderCreate(price);
                return Ok(orderId);
            }
            catch(DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("payment")]
        [Authorize]
        public  ActionResult Payment (PaymentDTO razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("razorpay details connot be null here");
                }
                var res = _service.RazorPayment(razorpay);
                return Ok(res);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);  
            }
        }
        [HttpPost("placeOrder")]
        [Authorize]
        public async Task<IActionResult>PlaceOrder(CreateOrderDTO createorderdto)
        {
            try
            {
                int UserId= Convert.ToInt32(HttpContext.Items["UserId"]);
                var res= await _service.CreateOrder(UserId, createorderdto);
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("getOrderDetails")]
        [Authorize]

        public async Task<IActionResult> GetOrderDetails()
        {
          try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.GetOrderDetails(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("get-order-details-admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOrderDetailsAdmin()
        {
            try
            {
                var res = await _service.GetOrderDetailsAdmin();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"An un expected error occured ",null,ex.Message));
            }
        }

        [HttpGet("GetOrderByUserId/{userId}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult>GetOrderByUserId(int userId)
        {
            try
            {
                var orderdetails=await _service.GetOrdersByUserId(userId);
                if (orderdetails == null)
                {
                    return NotFound(new ApiResponses<string>(404,"Details not found for the specified user"));
                }
                return Ok(new ApiResponses<List<ViewOrderUserDetailDTO>>(200,"Orderdetails fetched successully",orderdetails));
            }
            catch(Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An unexpected error occured",null,ex.Message));
            }
        }
        [HttpGet("totalrevenue")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> TotalRevenue()
        {
            try
            {
                var res =await _service.TotalRevenue();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("totalproductspurchased")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> TotalProductPurchased()
        {
            try
            {
                var res = await _service.TotalProductsPurchased();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("update-orderstatus")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult>UpdateOrder(string orderId,string orderStatus)
        {
            try
            {
                await _service.UpdateOrder(orderId, orderStatus);
                return Ok(new ApiResponses<string>(200, "OrderStatus Updated succcessfully"));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponses<string>(500,"an unexpected error occured",null,ex.Message));
            }
        }


    }
}
