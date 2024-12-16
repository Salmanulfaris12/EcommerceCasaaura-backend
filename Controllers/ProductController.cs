using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.ProductModels.ProductDTOs;
using CasaAura.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CasaAura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _service.GetAllProducts();
                return Ok(new ApiResponses<List<ProductDTO>>(200,"Product Fetched Successfully",products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Internal server Error",ex.Message));
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _service.GetProductById(id);
                if (product == null)
                {
                    return NotFound(new ApiResponses<string>(404,$"Product with {id} id not found "));
                }
                 return Ok(new ApiResponses<ProductDTO>(200,"Product fetched successfully",product));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An Unexpected Error occured",null,ex.Message));
            }
        }
        [HttpGet("GetByCategory")]
        public async Task<IActionResult>GetProductByCategory(int CategoryId)
        {
            try
            {
                var products=await _service.GetProductbyCategory(CategoryId);
                if (products.Count == 0)
                {
                    return NotFound(new ApiResponses<string>(404, $"product with {CategoryId} not found"));
                }
                return Ok(new ApiResponses<List<ProductDTO>>(200,"Product fetched successfully",products));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500,"An unexpected error occured",null,ex.Message));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProducts([FromForm] AddProductDTO newProduct, IFormFile image)
        {
            try
            {   
    
                await _service.AddProduct(newProduct,image);
                return Ok(new ApiResponses<string>(200,"Product added successfully"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected error occured", null, ex.Message));
            }
        }
        [HttpGet("search-item")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            try
            {
                var res = await _service.SearchProduct(search);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"An unexpected error occured",null,ex.Message));
            }
        }
        

        [HttpGet("paginated-products")]
        public async Task<IActionResult> pagination([FromQuery] int pageNumber = 1, [FromQuery] int size = 10)
        {
            try
            {
                var res = await _service.ProductPagination(pageNumber, size);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected Error occured", null, ex.Message));
            }
        }
        [Authorize(Roles ="admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult>RemoveDelete(int id)
        {
            try
            {
                bool res = await _service.RemoveProduct(id);
                if (res)
                {
                    return Ok(new ApiResponses<bool>(200, "Product removed successfully", res));
                }
                return NotFound(new ApiResponses<bool>(404, "Product not found ", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected error occured", null, ex.Message));
            }
        }
        [Authorize(Roles ="admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult>UpdateProduct(int id, [FromForm] AddProductDTO productdto,IFormFile image = null)
        {
            try
            {
                await _service.UpdateProduct(id, productdto, image);
                return Ok(new ApiResponses<string>(200, "Product updated successfully "));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An unexpected error occured", null, ex.Message));
            }
        }

    }
}
