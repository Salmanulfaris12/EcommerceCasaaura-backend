using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.WishListModels.WishListDTOs;
using CasaAura.Services.WishListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _service;
        public WishListController(IWishListService service)
        {
            _service = service;
        }
        [HttpGet("GetWishList")]
        [Authorize]
        public async Task<IActionResult>GetWishList()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.GetWishList(userId);
                if (res.Count==0)
                {
                    return Ok(new ApiResponses<List<WishListResDTO>>(200,"WishList is Empty",res));
                }
                return Ok(new ApiResponses<List<WishListResDTO>>(200,"WishList Fetched Successfully",res));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal Server Error",null,ex.Message));
            }
        }
        [HttpPost("AddorRemove/{productId}")]
        [Authorize]
        public async Task<IActionResult>AddOrRemove(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.AddorRemove(userId, productId);
                if (res == "Product does not exist.")
                {
                    return NotFound(new ApiResponses<string>(404, res));
                }
                return Ok(new ApiResponses<string>(200,res));

            }catch(Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"Internal Server Error",null,ex.Message));
            }
        }

    }
}
