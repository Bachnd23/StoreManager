using COCOApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace COCOApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreManagerContext _context;

        public OrderRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public List<Order> GetOrders()
        {
            return _context.Orders
                           .Include(o => o.Customer)
                           .Include(o => o.Product)
                           .AsQueryable()
                           .ToList();
        }

        public List<Order> GetOrdersByIds(List<int> orderIds, int sellerId)
        {
            var query = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .Where(o => orderIds.Contains(o.Id))
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }

            return query.ToList();
        }

        public Order GetOrderById(int orderId, int sellerId)
        {
            var query = _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }

            return orderId > 0 ? query.FirstOrDefault(u => u.Id == orderId) : null;
        }

        public List<Order> GetOrders(string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
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
                         .Include(o => o.Product)
                         .OrderByDescending(o => o.Id);

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
            var existingOrder = _context.Orders.FirstOrDefault(c => c.Id == orderId);

            if (existingOrder != null)
            {
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.ProductId = order.ProductId;
                existingOrder.Volume = order.Volume;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.UpdatedAt = order.UpdatedAt;

                _context.SaveChanges();
            }
            else
            {
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
                Debug.WriteLine($"Error retrieving orders: {ex.Message}");
                throw new ApplicationException("Error retrieving orders", ex);
            }
        }
    }
}
