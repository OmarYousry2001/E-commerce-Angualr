using Domains.AppMetaData;
using Domains.Entities;
using Domains.Entities.Identity;
using Domains.Entities.Product;
using Domains.Identity;
using Domains.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    public class ContextConfigurations
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";
        private static readonly string seedAdminPassword = "Admin-123";

        public static async Task SeedDataAsync(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager)
        {
            // Seed user first to get admin user ID
            var adminUserId = await SeedUserAsync(userManager, roleManager);

            // Seed E-commerce data
            await SeedProductDataAsync(context, adminUserId);
        }
        
    private static async Task SeedProductDataAsync(ApplicationDbContext context, Guid adminUserId)
        {
            // 1. Seed Categories
            if (!context.Category.Any())
            {
                var categories = new List<Category>
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Laptops",
                Description = "High performance laptops for all needs",
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Tablets",
                Description = "Portable tablets for work and play",
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Smartphones",
                Description = "Latest smartphones with powerful features",
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            }
        };

                await context.Category.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // 2. Seed Products
            if (!context.Product.Any())
            {
                var laptopCategory = await context.Category.FirstOrDefaultAsync(c => c.Name == "Laptops");
                var tabletCategory = await context.Category.FirstOrDefaultAsync(c => c.Name == "Tablets");
                var phoneCategory = await context.Category.FirstOrDefaultAsync(c => c.Name == "Smartphones");

                var products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "MacBook Pro M2",
                Description = "Powerful Apple laptop with M2 chip",
                NewPrice = 1999.99m,
                OldPrice = 2199.99m,
                rating = 4.8,
                CategoryId = laptopCategory.Id,
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "iPad Air 5",
                Description = "Slim and powerful tablet from Apple",
                NewPrice = 799.99m,
                OldPrice = 899.99m,
                rating = 4.6,
                CategoryId = tabletCategory.Id,
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Samsung Galaxy S24",
                Description = "Flagship Android phone with great camera",
                NewPrice = 1099.99m,
                OldPrice = 1199.99m,
                rating = 4.7,
                CategoryId = phoneCategory.Id,
                CurrentState = 1,
                CreatedBy = adminUserId,
                CreatedDateUtc = DateTime.UtcNow
            }
        };

                await context.Product.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // 3. Seed Photos
            if (!context.Photo.Any())
            {
                var allProducts = await context.Product.ToListAsync();

                var photos = allProducts.Select(p => new Photo
                {
                    Id = Guid.NewGuid(),
                    ProductId = p.Id,
                    ImagePath = $"images/products/{p.Name.ToLower().Replace(" ", "-")}.webp",
                    CurrentState = 1,
                    CreatedBy = adminUserId,
                    CreatedDateUtc = DateTime.UtcNow
                }).ToList();

                await context.Photo.AddRangeAsync(photos);
                await context.SaveChangesAsync();
            }

            // 4. (Optional) Seed Ratings - fake data for testing
            if (!context.Rating.Any())
            {
                var products = await context.Product.ToListAsync();

                var ratings = new List<Rating>();

                foreach (var product in products)
                {
                    ratings.Add(new Rating
                    {
                        Id = Guid.NewGuid(),
                        Stars = 5,
                        content = $"Amazing {product.Name}!",
                        ApplicationUserId = adminUserId.ToString(),
                        ProductId = product.Id,
                        CurrentState = 1,
                        CreatedBy = adminUserId,
                        CreatedDateUtc = DateTime.UtcNow
                    });
                }

                await context.Rating.AddRangeAsync(ratings);
                await context.SaveChangesAsync();
            }

            // 5. Seed DeliveryMethods
            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethods = new List<DeliveryMethod>
    {
        new DeliveryMethod
        {
            Id = Guid.NewGuid(),
            Name = "Standard Shipping",
            Price = 49.99m,
            DeliveryTime = "5-7 business days",
            Description = "Reliable and affordable shipping method",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new DeliveryMethod
        {
            Id = Guid.NewGuid(),
            Name = "Express Delivery",
            Price = 99.99m,
            DeliveryTime = "1-2 business days",
            Description = "Fast shipping for urgent needs",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        },
        new DeliveryMethod
        {
            Id = Guid.NewGuid(),
            Name = "Free Pickup",
            Price = 0.00m,
            DeliveryTime = "Same day pickup",
            Description = "Pickup your order from the nearest branch",
            CurrentState = 1,
            CreatedBy = adminUserId,
            CreatedDateUtc = DateTime.UtcNow
        }
    };

                await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                await context.SaveChangesAsync();
            }

        }


        private static async Task<Guid> SeedUserAsync(UserManager<ApplicationUser> userManager,
                   RoleManager<Role> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new Role { Name = Roles.User });
            }
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new Role { Name = Roles.Admin });
            }

            // Ensure admin user exists
            var adminEmail = seedAdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var id = Guid.NewGuid().ToString();
                adminUser = new ApplicationUser
                {
                    Id = id,
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName="Admin",
                    EmailConfirmed = true,
                    CreatedDateUtc = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, seedAdminPassword);
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }

            // Convert adminUser.Id from string to Guid
            return Guid.Parse(adminUser.Id);  // Convert to Guid
        }
    }
}
