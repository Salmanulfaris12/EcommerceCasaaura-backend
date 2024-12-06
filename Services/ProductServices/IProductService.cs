using CasaAura.Models.ProductModels.ProductDTOs;

namespace CasaAura.Services.ProductServices
{
    public interface IProductService
    {
        Task AddProduct(AddProductDTO productdto,IFormFile image);
        Task<List<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<List<ProductDTO>>GetProductbyCategory(int categoryId);
        Task<List<ProductDTO>>SearchProduct(string search);
        Task<List<ProductDTO>>ProductPagination(int pagenumber=1, int pageSize=10); 
        Task<bool>RemoveProduct(int id);
        Task UpdateProduct(int id ,AddProductDTO productdto,IFormFile image);

    }
}
