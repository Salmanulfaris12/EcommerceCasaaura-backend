using CasaAura.Models.ApiResposeModel;
using CasaAura.Models.CartModels;
using CasaAura.Models.CartModels.CartDTOs;
using CasaAura.Models.UserModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace CasaAura.Services.CartServices
{
    public class CartService:ICartService
    {
        protected readonly AppDbContext _context;
        public CartService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CartViewDTO>>GetCartItems(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    throw new Exception("User id is null");
                }
                var user = await _context.Carts.Include(c => c.CartItems).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (user != null)
                {
                    return user.CartItems.Select(x => new CartViewDTO
                    {
                        ProductId = x.ProductId,
                        ProductName = x.Product.ProductName,
                        Price = x.Product.ProductPrice,
                        Quantity = x.Quantity,
                        TotalAmount = x.Quantity * x.Product.ProductPrice,
                        Image = x.Product.Image

                    }).ToList();
                }
                return new List<CartViewDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponses<CartItem>>AddToCart(int userId,int productId)
        {
            try
            {
                var user=await _context.Users.Include(c=>c.Cart)
                    .ThenInclude(c=>c.CartItems)
                    .ThenInclude(c=>c.Product)
                    .FirstOrDefaultAsync(x=>x.Id== userId);
                if (user == null)
                {
                    return new ApiResponses<CartItem>(404, "User not found");
                }
                var product=await _context.Products.FirstOrDefaultAsync(p=>p.ProductId==productId);
                if (product == null)
                {
                    return new ApiResponses<CartItem>(404, $"Product with {productId} is not found");
                }
                if (product?.Stock <= 0)
                {

                    return new ApiResponses<CartItem>(404, "Out of stock");
                }
                if (product!=null && user != null)
                {
                    if (user.Cart == null)
                    {
                        user.Cart = new Cart
                        {
                            UserId = userId,
                            CartItems = new List<CartItem>()
                        };
                        _context.Carts.Add(user.Cart);
                       await _context.SaveChangesAsync();
                        
                    }
                }
                var check=user.Cart.CartItems.FirstOrDefault(x=>x.ProductId==productId);
                if (check != null)
                {
                    if (check.Quantity < 10)
                    {
                        check.Quantity++;
                        await _context.SaveChangesAsync();
                        return new ApiResponses<CartItem>(200,"Product Quantity increased successfully");
                    }
                }
                var item = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                user.Cart.CartItems.Add(item);
                await _context.SaveChangesAsync();
                return new ApiResponses<CartItem>(200,"Product added successfully");


            }
            catch (Exception ex)
            {
                return new ApiResponses<CartItem>(500, "Internal Server Error Occured");
            }
        }
        public async Task<bool>RemoveFromCart(int userId ,int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
               .ThenInclude(u => u.CartItems)
               .ThenInclude(u => u.Product)
               .FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (user==null)
                {
                    throw new Exception("user not found");
                }
                if (product == null)
                {
                    throw new Exception("product not found");
                }
                if (user!=null && product!=null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);
                    if (item != null)
                    {
                        user.Cart.CartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponses<CartItem>> IncreaseQty(int userId ,int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return new ApiResponses<CartItem>(404, "User not found");
                }
                var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                if (product == null)
                {
                    return new ApiResponses<CartItem>(404, "Product not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);
                if(item==null)
                {
                    return new ApiResponses<CartItem>(404,"Item not Found");
                }
                if (item.Quantity >= 10)
                {
                    return new ApiResponses<CartItem>(400,"Maximum Quntity reached (10 items)");
                }
                if (product.Stock <= item.Quantity)
                {
                    return new ApiResponses<CartItem>(400, "Out of stock!");
                }

                item.Quantity++;
                await _context.SaveChangesAsync();
                return new ApiResponses<CartItem>(200,"Quatity incresed successfully");
                
                
                
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DecreaseQty(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart).ThenInclude(c => c.CartItems).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (user != null && product != null)
                {
                    var item = user.Cart?.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
                    if (item != null)
                    {
                        if (item.Quantity > 1)
                        {
                            item.Quantity--;
                        }
                        else if(item.Quantity==1)
                        {
                            user.Cart.CartItems.Remove(item);
                        }

                        await _context.SaveChangesAsync();

                        return true;
                    }
                }
                return false;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
