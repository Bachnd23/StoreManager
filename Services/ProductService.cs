using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class ProductService : StoreManagerService
    {
        public List<Product> GetProducts()
        {
            var query = _context.Products.AsQueryable();
            return query.ToList();
        }
        public List<Product> GetProducts(string nameQuery, int pageNumber, int pageSize)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.ProductName.Contains(nameQuery));
            }
            query = query.OrderByDescending(p => p.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public int GetTotalProducts(string nameQuery)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.ProductName.Contains(nameQuery));
            }

            return query.Count();
        }
    }
}
