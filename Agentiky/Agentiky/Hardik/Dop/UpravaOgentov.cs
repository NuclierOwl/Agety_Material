using Agents_BD_Tres.Hardik.Connect;
using Agents_BD_Tres.Hardik.Connect.Dao;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agentiky.Hardik.Dop
{
    public class UpravaOgentov
    {
        private readonly Connecter _context;

        public UpravaOgentov()
        {
            _context = new Connecter();
            _context.Database.EnsureCreated();
        }


        public async Task<List<ProductDao>> GetAllProductsAsync() =>
            await _context.product.ToListAsync();

        public async Task<ProductDao> GetProductByIdAsync(int id) =>
            await _context.product.FirstOrDefaultAsync(p => p.id == id);

        public async Task AddProductAsync(ProductDao product)
        {
            await _context.product.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductDao product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product != null)
            {
                _context.product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        public List<ProductDao> GetAllProducts() =>
            _context.product.ToList();

        public ProductDao GetProductById(int id) =>
            _context.product.FirstOrDefault(p => p.id == id);

        public void AddProduct(ProductDao product)
        {
            _context.product.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(ProductDao product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.product.Find(id);
            if (product != null)
            {
                _context.product.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<ProductDao> FilterProducts(string searchTerm) =>
            _context.product
                .Where(p => p.title.Contains(searchTerm) ||
                           p.description.Contains(searchTerm))
                .ToList();
    }
}