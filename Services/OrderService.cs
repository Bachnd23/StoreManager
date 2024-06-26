using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class OrderService : StoreManagerService
    {
        public List<Order> GetOrders(string nameQuery, int pageNumber, int pageSize)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(o => o.Customer.Name.Contains(nameQuery));
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public int GetTotalOrders(string nameQuery)
        {

            var query = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Customer.Name.Contains(nameQuery));
            }

            return query.Count();
        }
    }
}
