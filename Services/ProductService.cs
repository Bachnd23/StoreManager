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
        public Product GetProductById(int id,int sellerId)
        {
            var query = _context.Products.AsQueryable();
            if(sellerId > 0)
            {
                query=query.Where(p=> p.SellerId == sellerId);
            }
            return query.FirstOrDefault(p=>p.Id==id);
        }
        public List<Product> GetProducts(string nameQuery, int pageNumber, int pageSize,int sellerId)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Products.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(p => p.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.ProductName.Contains(nameQuery));
            }
            query = query.OrderByDescending(p => p.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public int GetTotalProducts(string nameQuery,int sellerId)
        {
            var query = _context.Products.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(p => p.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.ProductName.Contains(nameQuery));
            }
            return query.Count();
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}
