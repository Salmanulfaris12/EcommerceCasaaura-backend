using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.UserModels.DTOs;
using CasaAura.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet("GetUsers")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _service.GetUsers();
                var res = new ApiResponses<IEnumerable<UserViewDTO>>(200, "Users retrieved successfully", users);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An unexpected error occured",null ,ex.Message));
            }
        }

        [HttpGet("{UserId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult>GetUser(int UserId)
        {
            try
            {
                var user = await _service.GetUserById(UserId);
                if (user == null)
                {
                    return NotFound(new ApiResponses<string>(404, "User not found ", null));
                }
                var res = new ApiResponses<UserViewDTO>(200, "user retrieved successfully ", user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An unexpected error occured",null,ex.Message));
            }
        }
        [HttpPatch("BlockOrUnBlock/{UserId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BlockOrUnBlock(int UserId)
        {
            try
            {
                var result = await _service.BlockAndUnBlock(UserId);
                var res = new ApiResponses<BlockUnblockRes>(200, "Block/unBlock action performed successfully",result);
                return Ok(res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected eror occured", null, ex.Message));
            }
        }



    }
}
