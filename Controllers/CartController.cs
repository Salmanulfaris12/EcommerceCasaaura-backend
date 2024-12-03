using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.CartModels.CartDTOs;
using CasaAura.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }
        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res= await _service.GetCartItems(userId);
                if (res.Count == 0)
                {
                    return Ok(new ApiResponses<IEnumerable<CartViewDTO>>(200,"Cart is Empty",res));
                }
                return Ok(new ApiResponses<IEnumerable<CartViewDTO>>(200,"Cart Fetched Successfully",res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Inteernal Server Error",null,ex.Message));
            }
        }
        [HttpPost("Add/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.AddToCart(userId, productId);
                if (res.StatusCode==200)
                {
                    return Ok(res);
                }
                if (res.StatusCode == 404)
                {
                    return NotFound(new ApiResponses<string>(404,res.Message));
                }
                return BadRequest(new ApiResponses<string>(400,"Bad request"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal server Error",null,ex.Message));
            }

        }
        [HttpDelete("Delete/{productId}")]
        [Authorize]
        public async Task<IActionResult>RemoveFromCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res= await _service.RemoveFromCart(userId,productId);           
                if (!res)
                {
                    return BadRequest(new ApiResponses<string>(400,"Item not found in cart",null,"Item not found in Cart"));
                }
                return Ok(new ApiResponses<string>(200,"Item deleted Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal Server",null,ex.Message));
            }
        }
        [HttpPut("IncreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult>IncreaseQty(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res=await _service.IncreaseQty(userId,productId);
                if (res.StatusCode==404)
                {
                    return NotFound(new ApiResponses<string>(404,res.Message,null,res.Message));
                }
                if (res.StatusCode == 400)
                {
                    return BadRequest(new ApiResponses<string>(400,res.Message,null,res.Message));
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal Server Error"));
            }
        }
        [HttpPut("DecreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult>DecreaseQty(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.DecreaseQty(userId, productId);
                if (!res)
                {
                    return BadRequest(new ApiResponses<string>(400,"Item not Found in Cart ",null,"Item not Found in Cart"));
                }
                return Ok(new ApiResponses<string>(200,"Quantity decreased successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"Internal Server Error",null,ex.Message));
            }
        }

    }
}
