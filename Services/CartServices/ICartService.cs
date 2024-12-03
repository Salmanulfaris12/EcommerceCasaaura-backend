using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.CartModels;
using CasaAura.Models.CartModels.CartDTOs;

namespace CasaAura.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartViewDTO>> GetCartItems(int userId);
        Task<ApiResponses<CartItem>> AddToCart(int userId, int productId);
        Task<bool> RemoveFromCart(int userId, int productId);
        Task<ApiResponses<CartItem>>IncreaseQty(int userId, int productId);
        Task<bool>DecreaseQty(int userId, int productId);
    }
}
