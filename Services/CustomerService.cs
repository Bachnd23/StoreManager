using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class CustomerService : StoreManagerService
    {
        public List<Customer> GetCustomers(string nameQuery, int pageNumber, int pageSize)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int GetTotalCustomers(string nameQuery)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Name.Contains(nameQuery));
            }

            return query.Count();
        }
    }
}
