using HomeWork4Products.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeWork4Products.Services
{
    public interface IServiceProducts
    {
        Task<Product?> CreateAsync(Product? product);
        Task<IEnumerable<Product>?> ReadAsync(string searchString);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> UpdateAsync(int id, Product? product);
        Task<bool> DeleteAsync(int id);
    }
    public class ServiceProducts : IServiceProducts
    {
        private readonly ProductContext _productContext;

        public ServiceProducts(ProductContext productContext)
        {
            _productContext = productContext;
        }
        public async Task<IEnumerable<Product>?> ReadAsync(string searchString)
        {
            var products = from p in _productContext.Product
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => ((p.Name + " " + p.Description).ToUpper().Contains(searchString.ToUpper())));
            }
            return await products.ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productContext.Product.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product?> CreateAsync(Product? product)
        {
            _productContext.Product.Add(product);
            await _productContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> UpdateAsync(int id, Product? product)
        {
            if (id != product?.Id)
            {
                return null;
            }
            _productContext.Product.Update(product);
            await _productContext.SaveChangesAsync();
            return product;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productContext.Product.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return false;
            }
            _productContext.Product.Remove(product);
            await _productContext.SaveChangesAsync();
            return true;
        }

    }
}
