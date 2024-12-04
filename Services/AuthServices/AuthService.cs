using AutoMapper;
using CasaAura.Models.UserModels;
using CasaAura.Models.UserModels.DTOs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CasaAura.Services.AuthServices
{
    public class AuthService:IAuthService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IMapper mapper,ILogger<AuthService> logger,AppDbContext context,IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }
        public async Task<bool> Register(UserRegisterDTO newUser)
        {
            try
            {
                if (newUser == null)
                {
                    throw new ArgumentNullException("User data cannnot be null");
                }
                var IsUserexist = await _context.Users.SingleOrDefaultAsync(u => u.Email == newUser.Email);
                if (IsUserexist != null)
                {
                    return false;
                }

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                //newUser.Password = hashPassword;

                var user = _mapper.Map<User>(newUser);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception($"Validation error: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"database error:{ex.InnerException?.Message??ex.Message}");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<UserResDTO> Login(UserLoginDTO userdto)
        {
            try
            {
                _logger.LogInformation("Logging into the user");

                var usr = await _context.Users.FirstOrDefaultAsync(u => u.Email == userdto.Email);
                if (usr == null)
                {
                    _logger.LogWarning("user not found");
                    return new UserResDTO { Error = "Not Found" };
                }

                _logger.LogInformation("validating email...");
                var pass = ValidatePassword(userdto.Password,usr.Password);

                if (!pass)
                {
                    _logger.LogWarning("invalid password");
                    return new UserResDTO { Error = "Invalid Password" };
                }

                if (usr.IsBlocked)
                {
                    _logger.LogWarning("user is blocked");
                    return new UserResDTO { Error = "User Blocked" };
                }

                _logger.LogInformation("generating token");
                var token = GenarateToken(usr);
                return new UserResDTO
                {
                    Token = token,
                    Role = usr.Role,
                    Email = usr.Email,
                    Id = usr.Id,
                    Name=usr.Name

                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in login:{ex.Message}");
                throw;
            }
        }
        private string GenarateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials= new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var token = new JwtSecurityToken(
                claims:claim,
                signingCredentials:credentials,
                expires:DateTime.UtcNow.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public bool ValidatePassword(string password,string hashpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashpassword);
        }
    }
}
