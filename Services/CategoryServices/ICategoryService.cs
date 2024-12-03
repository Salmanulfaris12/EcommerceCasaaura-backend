using CasaAura.Models.CategoryModels.CategoryDTOs;

namespace CasaAura.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<bool> AddCategory(CategoryDTO category);
        Task<bool> RemoveCategory(int id);
    }
}
