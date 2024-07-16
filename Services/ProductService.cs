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
        public Product GetProductById(int productId,int sellerId)
        {
            var query = _context.Products.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (productId > 0)
            {
                return query.FirstOrDefault(u => u.Id == productId);
            }
            else
            {
                return null;
            }
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
        public void EditProduct(int productId, Product product)
        {
            // Retrieve the customer from the database
            Product? existingProduct = _context.Products.FirstOrDefault(c => c.Id == productId);

            // Check if the customer exists
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Cost = product.Cost;
                existingProduct.Status = product.Status;
                existingProduct.SellerId= product.SellerId; 
                existingProduct.UpdatedAt = product.UpdatedAt;   

                // Save the changes to the database
                _context.SaveChanges();
            }
            else
            {
                // Handle the case when the product is not found
                throw new ArgumentException("Product not found");
            }
        }
    }
}
