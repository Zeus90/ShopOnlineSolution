using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;

namespace ShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(ShopOnlineContext context)
        {
            _context = context;
        }

        public readonly ShopOnlineContext _context;

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _context.ProductCategories.ToListAsync();

            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var pCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            return pCategory;
        }

        public async Task<Product> GetItem(int id)
        {
            var itemProduct = await _context.Products.Include(p => p.ProductCategory).SingleOrDefaultAsync(
                p => p.Id == id);

            return itemProduct;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _context.Products.Include(p => p.ProductCategory).ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = await _context.Products.Include(p => p.ProductCategory).Where(p => p.CategoryId == id).ToListAsync();

            return products;
        }
    }
}
