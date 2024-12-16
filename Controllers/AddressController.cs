using CasaAura.Models.AddressModels.AddresDTOs;
using CasaAura.Models.ApiResposeModel;
using CasaAura.Services.AddressServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }
        [HttpPost("AddAddress")]
        [Authorize]
        public async Task<IActionResult>AddAddress(AddressCreateDTO addressDTO)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res= await _service.AddAddress(userId, addressDTO);
                if (res)
                {
                    return Ok(new ApiResponses<string>(200, "Address added successfully"));
                }
                return BadRequest(new ApiResponses<string>(400,"failed",null,"error occured while adding address"));
            }catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An unexpected error occured",null,ex.Message));
            }
        }
        [HttpGet("getAddresses")]
        [Authorize]
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                int userId =Convert.ToInt32(HttpContext.Items["UserId"]);
                var res =await _service.GetAddresses(userId);
                if (res != null)
                {
                    return Ok(new ApiResponses<List<AddressResDTO>>(200, "Addresses fetched successfully", res));
                }
                return BadRequest(new ApiResponses<string>(400,"failed to fetch address" ));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected error occured", null, ex.Message));
            }
        }
        [HttpDelete("DeleteAddress")]
        [Authorize]
        public async Task<IActionResult>RemoveAddress(int addressId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _service.RemoveAddress(userId,addressId);
                if (res)
                {
                    return Ok(new ApiResponses<string>(200, "Address removed successfully"));
                }
                return BadRequest(new ApiResponses<string>(400, "failed to delete address"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected error occured", null, ex.Message));
            }
        }
    }
}
