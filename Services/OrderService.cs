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
        public List<Order> GetOrdersByIds(List<int> orderIds, int sellerId)
        {

            var query = _context.Orders.AsQueryable();

            query = query.Include(o => o.Customer)
                         .Include(o => o.Product)
                         .Where(o => orderIds.Contains(o.Id));
            return query.ToList();
        }
        public Order GetOrderById(int orderId, int sellerId)
        {
            var query = _context.Orders.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            query = query.Include(o => o.Customer)
                         .Include(o => o.Product);
            if (orderId > 0)
            {
                return query.FirstOrDefault(u => u.Id == orderId);
            }
            else
            {
                return null;
            }
        }
        public List<Order> GetOrders(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Orders.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(o => o.Customer.Name.Contains(nameQuery));
            }
            query = query.Include(o => o.Customer)
                         .Include(o => o.Product);
            query = query.OrderByDescending(o => o.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public int GetTotalOrders(string nameQuery, int sellerId)
        {

            var query = _context.Orders.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Customer.Name.Contains(nameQuery));
            }
            query = query.Include(o => o.Customer)
                         .Include(o => o.Product);
            return query.Count();
        }
        public List<SelectListItem> GetCustomersSelectList(int sellerId)
        {
            var query = _context.Customers.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(c => c.SellerId == sellerId);
            }
            return query.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

        public List<SelectListItem> GetProductsSelectList(int sellerId)
        {
            var query = _context.Products.AsQueryable();
            if (sellerId > 0)
            {
                query = query.Where(p => p.SellerId == sellerId);
            }
            return query.Select(i => new SelectListItem
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
        public void EditOrder(int orderId, Order order)
        {
            // Retrieve the customer from the database
            Order? existingOrder = _context.Orders.FirstOrDefault(c => c.Id == orderId);

            // Check if the customer exists
            if (existingOrder != null)
            {
                existingOrder.CustomerId=order.CustomerId;
                existingOrder.ProductId=order.ProductId;
                existingOrder.Volume=order.Volume;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.UpdatedAt=order.UpdatedAt;

                // Save the changes to the database
                _context.SaveChanges();
            }
            else
            {
                // Handle the case when the product is not found
                throw new ArgumentException("Order not found");
            }
        }
        public List<Order> GetOrders(string dateRange, int customerId, int sellerId)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                var dateRangeParts = dateRange.Split(" - ");
                if (dateRangeParts.Length == 2)
                {
                    if (!DateTime.TryParse(dateRangeParts[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        startDate = DateTime.MinValue;
                    }
                    if (!DateTime.TryParse(dateRangeParts[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        endDate = DateTime.MaxValue;
                    }
                }
            }

            try
            {
                var query = _context.Orders.AsQueryable();
                if (sellerId > 0)
                {
                    query = query.Where(o => o.SellerId == sellerId);
                }
                query = query.Include(o => o.Customer)
                             .Include(o => o.Product)
                             .Where(o => o.CustomerId == customerId && o.OrderDate >= startDate && o.OrderDate <= endDate);
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
