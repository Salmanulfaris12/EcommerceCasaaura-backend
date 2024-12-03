using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.UserModels.DTOs;
using CasaAura.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService,ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO newUser)
        {
          
            try
            {

                if (newUser == null)
                {
                    return BadRequest(new ApiResponses<string>(400, "User data cannot be null"));
                }
                bool isDone = await _authService.Register(newUser);
                if (!isDone)
                {
                    return Conflict(new ApiResponses<string>(409,"User already exist"));
                }
                return Ok(new ApiResponses<bool>(200,"User Registered successfully",isDone));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An un expected error occured",null,ex.Message));
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO usrlog)
        {
            try
            {
                var res = await _authService.Login(usrlog);
                _logger.LogInformation($"result is{res}");
                if(res.Error=="Not Found")
                {
                    return NotFound(new ApiResponses<string>(404,"Not Found",null,"Please SignUp,User not found"));
                }
                if(res.Error=="User Blocked")
                {
                    return StatusCode(403,new ApiResponses<string>(403, "Forbidden", null, "User is Blocked by Admin"));
                }
                if (res.Error=="Invalid Password")
                {
                    return BadRequest(new ApiResponses<string>(400,"BadRequest",null,res.Error));
                }
                return Ok(new ApiResponses<UserResDTO>(200,"Login Successfull",res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal server error occured",null,ex.Message));
            }
        }

    }
}
