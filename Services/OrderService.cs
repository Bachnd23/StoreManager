using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;

namespace COCOApp.Services
{
    public class OrderService : StoreManagerService
    {
        public List<Order> GetOrders()
        {

            var query = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .AsQueryable();
            return query.ToList();
        }
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
            query = query.OrderByDescending(o => o.Id);
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
        public List<SelectListItem> GetCustomersSelectList()
        {
            return _context.Customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

        public List<SelectListItem> GetProductsSelectList()
        {
            return _context.Products.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.ProductName
            }).ToList();
        }
        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public List<Order> GetOrders(string dateRange, int customerId)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                var dateRangeParts = dateRange.Split(" - ");
                if (dateRangeParts.Length == 2)
                {
                    if (!DateTime.TryParseExact(dateRangeParts[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        startDate = DateTime.MinValue;
                    }
                    if (!DateTime.TryParseExact(dateRangeParts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        endDate = DateTime.MaxValue;
                    }
                }
            }

            try
            {
                var query = _context.Orders
                                    .Include(o => o.Customer)
                                    .Include(o => o.Product)
                                    .Where(o => o.CustomerId == customerId && o.Date >= startDate && o.Date <= endDate);

                return query.ToList();
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Debug.WriteLine($"Error retrieving reports: {ex.Message}");
                throw new ApplicationException("Error retrieving reports", ex);
            }
        }
    }
}
