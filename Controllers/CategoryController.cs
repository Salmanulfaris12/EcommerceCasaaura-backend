using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.CategoryModels.CategoryDTOs;
using CasaAura.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var CategoryList = await _service.GetCategories();
                return Ok(new ApiResponses<List<CategoryResDTO>>(200,"Categories retrieved successfully",CategoryList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal server error",null,ex.Message));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddCategory([FromForm]CategoryDTO categorydto,IFormFile image)
        {
            try
            {
                var res = await _service.AddCategory(categorydto,image);
                if (res)
                {
                    return Ok(new ApiResponses<bool>(200,"Category added successfully",res));
                }
                return Conflict(new ApiResponses<string>(409,"Category already exist"));
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new ApiResponses<string>(500,"Internal Server Error",null,ex.Message));
                }
            }
        }
        [Authorize(Roles ="admin")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id )
        {
            try
            {
                var res =await _service.RemoveCategory(id);
                if (res)
                {
                    return Ok(new ApiResponses<bool>(200,"Item deleted from Category",res));
                }
                return NotFound(new ApiResponses<string>(404,"Item not Found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"Internal Server Error"));

            }
        }
    }
}
