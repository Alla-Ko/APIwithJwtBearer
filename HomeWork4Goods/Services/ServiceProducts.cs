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
        private readonly ILogger<ServiceProducts> _logger;
        public ServiceProducts(ProductContext productContext, ILogger<ServiceProducts> logger)
        {
            _productContext = productContext;
            _logger = logger;
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
            if (product == null)
            {
                _logger.LogWarning("Attempt to create product with null");
                return null;
            }
            _productContext.Product.Add(product);
            await _productContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> UpdateAsync(int id, Product? product)
        {
            if (product == null || id == null)
            {
                _logger.LogWarning("product == null||id==null");
                return null;
            }
            if (id != product?.Id)
            {
                _logger.LogWarning("id != product?.Id");
                return null;
            }
            try
            {
                _productContext.Product.Update(product);
                await _productContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productContext.Product.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning("Attempt to delete non-existent product");
                return false;
            }
            _productContext.Product.Remove(product);
            await _productContext.SaveChangesAsync();
            return true;
        }

    }
}
