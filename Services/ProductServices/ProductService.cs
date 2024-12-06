using AutoMapper;
using CasaAura.Models.ProductModels;
using CasaAura.Models.ProductModels.ProductDTOs;
using CasaAura.Services.CloudinaryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace CasaAura.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(AppDbContext context,IMapper mapper,ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            try
            {
                var products=await _context.Products.Include(x=>x.Category).ToListAsync();
                if(products.Count>0 )
                {
                    var productwithCategory = products.Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        ProductName=p.ProductName,
                        ProductDescription=p.ProductDescription,
                        ProductPrice=p.ProductPrice,
                        Category=p.Category.CategoryName,
                        Image=p.Image,
                        Material=p.Material,
                        MRP =p.MRP,
                        Stock=p.Stock

                    }).ToList();
                    return productwithCategory;
                }
                return new List<ProductDTO>();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null)
                {
                    return null;
                }
                return new ProductDTO
                {
                    ProductId=product.ProductId,
                    ProductName=product.ProductName,
                    ProductDescription=product.ProductDescription,
                    ProductPrice=product.ProductPrice,
                    Image=product.Image,
                    Category=product.Category.CategoryName,
                    Material=product.Material,
                    MRP =product.MRP,
                    Stock=product.Stock
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductDTO>> GetProductbyCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category)
                    .Where(x => x.Category.CategoryId == categoryId)
                    .Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        ProductDescription = p.ProductDescription,
                        ProductPrice = p.ProductPrice,
                        ProductName = p.ProductName,
                        Category = p.Category.CategoryName,
                        Image = p.Image,
                        Material = p.Material,
                        MRP = p.MRP,
                        Stock = p.Stock
                    }).ToListAsync();
                return products;
               
            }
            catch (Exception ex) { throw new Exception( ex.Message); }
        }
        public async Task AddProduct(AddProductDTO productdto,IFormFile image)
        {
            try
            {
                if (productdto == null) throw new Exception("Product cannot to be null");
                if (productdto.ProductPrice > productdto.MRP)
                {
                    throw new Exception("Productprice must be greater than or equal to MRP rate");
                }
                var categoryExist = _context.Categories.FirstOrDefault(x => x.CategoryId == productdto.CategoryId);
                if (categoryExist == null) throw new Exception("Category with this Id doesn't exist");
                var imageUrl=await _cloudinaryService.UploadImageAsync(image);
                var product = _mapper.Map<Product>(productdto);
                product.Image = imageUrl;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
               
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
               
                throw new Exception($"An Exception has been occuured while creating new product {ex.Message}");
            }

        }
        public async Task<List<ProductDTO>>SearchProduct(string searchterm)
        {
            if (string.IsNullOrEmpty(searchterm))
            {
                return new List<ProductDTO>();
            }
            var products= await _context.Products.Include(x=>x.Category)
                .Where(x=>x.ProductName.ToLower().Contains(searchterm.ToLower()))
                .ToListAsync();
            return products.Select(x => new ProductDTO
            {
                ProductId = x.ProductId,
                ProductName=x.ProductName,
                ProductDescription=x.ProductDescription,
                ProductPrice=x.ProductPrice,
                Category=x.Category.CategoryName,
                Material=x.Material,
                Image=x.Image,
                MRP = x.MRP,
                Stock = x.Stock
            }).ToList();
        }
        public async Task<List<ProductDTO>> ProductPagination(int pagenumber = 1, int pagesize = 10)
        {
            try
            {
                var products = await _context.Products
                    .Include(x => x.Category)
                    .Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize)
                    .ToListAsync();

                return products.Select(x => new ProductDTO
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductDescription = x.ProductDescription,
                    ProductPrice = x.ProductPrice,
                    Category = x.Category.CategoryName,
                    Material = x.Material,
                    Image = x.Image,
                    MRP = x.MRP,
                    Stock = x.Stock
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool>RemoveProduct(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task UpdateProduct(int id, AddProductDTO productdto,IFormFile image = null)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == productdto.CategoryId);
                if (categoryExist == null)
                {
                    throw new Exception("Categoy with this id not found");
                }
                if (productdto.ProductPrice > productdto.MRP)
                {
                    throw new Exception("Productprice must be greater than or equal to MRP rate");
                }
                if (product != null)
                {
                    product.ProductName = productdto.ProductName;
                    product.ProductDescription = productdto.ProductDescription;
                    product.ProductPrice = productdto.ProductPrice;
                    product.Material = productdto.Material;
                    product.CategoryId = productdto.CategoryId;

                    if (image == null)
                    {
                        product.Image = product.Image;
                    }
                    if (image != null && image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                        product.Image = imageUrl;
                    }
                    await _context.SaveChangesAsync();

                }
                else
                {
                    throw new Exception($"Product with {id} not found");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
