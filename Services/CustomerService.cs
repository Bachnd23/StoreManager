using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class CustomerService : StoreManagerService
    {
        public List<Customer> GetCustomers()
        {
            var query = _context.Customers.AsQueryable();
            return query.ToList();
        }
        public List<Customer> GetCustomers(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Customers
                        .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(c => c.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int GetTotalCustomers(string nameQuery, int sellerId)
        {
            var query = _context.Customers
                        .AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }

            return query.Count();
        }
        public void AddCustormer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

    }
}
