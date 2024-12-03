using CasaAura.Models.UserModels.DTOs;

namespace CasaAura.Services.AuthServices
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDTO userRegisterDTO);
        Task<UserResDTO> Login(UserLoginDTO userDTO);
    }
}
