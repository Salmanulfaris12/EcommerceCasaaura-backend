using AutoMapper;
using CasaAura.Models.CategoryModels;
using CasaAura.Models.CategoryModels.CategoryDTOs;
using CasaAura.Services.CloudinaryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CasaAura.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public CategoryService(AppDbContext context, IMapper mapper ,ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<CategoryResDTO>>GetCategories()
        {
            try
            {
                var ctgry = await _context.Categories.ToListAsync();
                var mappedCategories = ctgry.Select(x=>new CategoryResDTO
                {
                    Id=x.CategoryId,
                    Name=x.CategoryName,
                    Image=x.Image
                }).ToList();
                return mappedCategories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddCategory(CategoryDTO categorydto,IFormFile image)
        {
            try
            {
                var isExist = await _context.Categories.AnyAsync(c => c.CategoryName.ToLower() == categorydto.CategoryName.ToLower());
                if (!isExist)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                    var d = _mapper.Map<Category>(categorydto);
                    d.Image=imageUrl;
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
