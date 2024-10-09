using Microsoft.EntityFrameworkCore;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<HomeWork4Products.Models.Product> Product { get; set; } = default!;
}
