using AutoMapper;
using CasaAura.Models.CategoryModels;
using CasaAura.Models.CategoryModels.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasaAura.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>>GetCategories()
        {
            try
            {
                var ctgry = await _context.Categories.ToListAsync();
                return _mapper.Map<List<CategoryDTO>>(ctgry);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddCategory(CategoryDTO categorydto)
        {
            try
            {
                var isExist = await _context.Categories.AnyAsync(c => c.CategoryName.ToLower() == categorydto.CategoryName.ToLower());
                if (!isExist)
                {
                    var d = _mapper.Map<Category>(categorydto);
                    await _context.Categories.AddAsync(d);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool>RemoveCategory(int id)
        {
            try
            {
                var res = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                if(res == null)
                {
                    return false;
                }
                else
                {
                    _context.Categories.Remove(res);
                    await _context.SaveChangesAsync();
                    return true;
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
