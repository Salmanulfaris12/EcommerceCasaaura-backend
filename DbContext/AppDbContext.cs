using CasaAura.Models.CartModels;
using CasaAura.Models.CategoryModels;
using CasaAura.Models.OrderModels;
using CasaAura.Models.ProductModels;
using CasaAura.Models.UserModels;
using CasaAura.Models.WishListModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace CasaAura
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WishList>WishLists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderMain>Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        private readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionstring = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionstring);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("user");

            modelBuilder.Entity<User>()
                .Property(u => u.IsBlocked)
                .HasDefaultValue(false);

            //Product to category relation
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);

            //WishList to product relation
            modelBuilder.Entity<WishList>()
                .HasOne(w=>w.Products)
                .WithMany()
                .HasForeignKey(w=>w.ProductId);
            
            //Wishlist to user relation
            modelBuilder.Entity<WishList>()
                .HasOne(w=>w.Users)
                .WithMany(w=>w.WishLists)
                .HasForeignKey(w=>w.UserId);

            //user to cart relation
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(u => u.User)
                .HasForeignKey<Cart>(x => x.UserId);

            //cart to cartitem relation
            modelBuilder.Entity<Cart>()
                .HasMany(x => x.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(i => i.CartId);

            //CartItem to product relation
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany(c=>c.CartItems)
                .HasForeignKey(k => k.ProductId);


            modelBuilder.Entity<OrderMain>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(f => f.UserId);


            modelBuilder.Entity<OrderItem>()
                .HasOne(u => u.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(f => f.OrderId);


            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.productId);


            modelBuilder.Entity<OrderItem>()
                .Property(pr => pr.TotalPrice).
                HasPrecision(30, 2);

        }
    }
}
