using CasaAura.Models.WishListModels.WishListDTOs;

namespace CasaAura.Services.WishListServices
{
    public interface IWishListService
    {
        Task<string> AddorRemove(int userId, int productId);
        Task<List<WishListResDTO>> GetWishList(int userId);
    }
}
