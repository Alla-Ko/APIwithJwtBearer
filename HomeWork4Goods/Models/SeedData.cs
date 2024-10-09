using HomeWork4Products.Models;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ProductContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ProductContext>>()))
        {
            // Check if the database has been seeded
            if (context.Product.Any())
            {
                return;   // DB has been seeded
            }

            context.Product.AddRange(
                new Product
                {
                    Name = "Apple iPhone 14",
                    Description = "The latest iPhone model with 5G capability, A15 Bionic chip, and improved camera system.",
                    Price = 799.99M
                },
                new Product
                {
                    Name = "Samsung Galaxy S23",
                    Description = "Flagship smartphone from Samsung featuring a dynamic AMOLED display and top-notch performance.",
                    Price = 999.99M
                },
                new Product
                {
                    Name = "Sony WH-1000XM5",
                    Description = "Industry-leading noise-canceling wireless headphones with premium sound quality.",
                    Price = 399.99M
                },
                new Product
                {
                    Name = "Dell XPS 13 Laptop",
                    Description = "13-inch ultra-thin laptop with InfinityEdge display and powerful Intel i7 processor.",
                    Price = 1199.99M
                },
                new Product
                {
                    Name = "Apple MacBook Pro 16\"",
                    Description = "High-performance laptop with M1 Pro chip, Retina display, and long battery life.",
                    Price = 2499.99M
                },
                new Product
                {
                    Name = "Google Pixel 7",
                    Description = "The latest Google phone with an advanced AI-powered camera and seamless integration with Google services.",
                    Price = 599.99M
                },
                new Product
                {
                    Name = "Sony PlayStation 5",
                    Description = "Next-generation gaming console with 4K gaming and ultra-fast SSD for load times.",
                    Price = 499.99M
                },
                new Product
                {
                    Name = "Microsoft Xbox Series X",
                    Description = "Powerful gaming console with 12 teraflops of processing power and 4K gameplay.",
                    Price = 499.99M
                },
                new Product
                {
                    Name = "Bose QuietComfort Earbuds II",
                    Description = "Premium true wireless earbuds with advanced noise-canceling technology and crisp sound.",
                    Price = 299.99M
                },
                new Product
                {
                    Name = "Apple AirPods Pro 2",
                    Description = "Second-generation wireless earbuds with active noise cancellation and transparency mode.",
                    Price = 249.99M
                },
                new Product
                {
                    Name = "Fitbit Charge 5",
                    Description = "Advanced health and fitness tracker with built-in GPS, heart rate monitor, and stress tracking.",
                    Price = 179.99M
                },
                new Product
                {
                    Name = "Nikon Z7 II Camera",
                    Description = "Full-frame mirrorless camera with 45.7 MP resolution, dual processors, and 4K video recording.",
                    Price = 2999.99M
                },
                new Product
                {
                    Name = "Canon EOS R6",
                    Description = "Mirrorless camera with 20 MP resolution, advanced autofocus system, and 4K video support.",
                    Price = 2499.99M
                },
                new Product
                {
                    Name = "Dyson V15 Detect Vacuum",
                    Description = "Powerful cordless vacuum cleaner with laser dust detection and advanced filtration system.",
                    Price = 749.99M
                },
                new Product
                {
                    Name = "Instant Pot Duo 7-in-1",
                    Description = "Multifunctional pressure cooker with seven cooking modes, including slow cooking and steaming.",
                    Price = 99.99M
                },
                new Product
                {
                    Name = "KitchenAid Stand Mixer",
                    Description = "Classic stand mixer with 10 speeds and durable build, perfect for baking and cooking tasks.",
                    Price = 499.99M
                },
                new Product
                {
                    Name = "GoPro HERO11 Black",
                    Description = "Waterproof action camera with 5.3K video recording, 27 MP photos, and HyperSmooth stabilization.",
                    Price = 499.99M
                },
                new Product
                {
                    Name = "Nintendo Switch OLED",
                    Description = "Hybrid gaming console with a 7-inch OLED screen, detachable controllers, and extensive game library.",
                    Price = 349.99M
                },
                new Product
                {
                    Name = "Logitech MX Master 3",
                    Description = "Wireless ergonomic mouse with precision scrolling, customizable buttons, and multi-device support.",
                    Price = 99.99M
                },
                new Product
                {
                    Name = "Razer DeathAdder V2",
                    Description = "High-precision gaming mouse with 20K DPI sensor, optical switches, and ergonomic design.",
                    Price = 69.99M
                }
            );

            context.SaveChanges();
        }
    }
}
