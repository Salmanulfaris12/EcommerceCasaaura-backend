using CasaAura.Models.CategoryModels.CategoryDTOs;

namespace CasaAura.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryResDTO>> GetCategories();
        Task<bool> AddCategory(CategoryDTO category,IFormFile image);
        Task<bool> RemoveCategory(int id);
    }
}
