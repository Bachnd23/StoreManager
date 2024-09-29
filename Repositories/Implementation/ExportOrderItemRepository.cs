using COCOApp.Models;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace COCOApp.Repositories
{
    public class ExportOrderItemRepository : IExportOrderItemRepository
    {
        private readonly StoreManagerContext _context;

        public ExportOrderItemRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public void addExportOrderItem(ExportOrderItem item)
        {
            ExportOrderItem exportOrderItem = _context.ExportOrderItems.FirstOrDefault(o => o.OrderId == item.OrderId && o.ProductId == item.ProductId);
            if (exportOrderItem == null)
            {
                _context.ExportOrderItems.Add(item);
            }
            else
            {
                exportOrderItem.Volume += item.Volume;
                Product product = _context.Products.FirstOrDefault(p => p.Id == exportOrderItem.ProductId);
                exportOrderItem.Total=exportOrderItem.Volume*product.Cost;
            }
            _context.SaveChanges();
        }

        public ExportOrderItem GetExportOrderItemById(int orderId, int productId, int sellerId)
        {
            var query = _context.ExportOrderItems
                                .Include(o => o.Product)
                                .Include(o => o.Order)
                                .ThenInclude(c => c.Customer)
                                .AsQueryable();

            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            ExportOrderItem item = query.FirstOrDefault(u => u.OrderId == orderId && u.ProductId == productId);
            return item;
        }

        public List<ExportOrderItem> GetExportOrderItems(int orderId,string nameQuery, int pageNumber, int pageSize, int sellerId)
        {
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.ExportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Customer);
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(o => o.Order.Customer.Name.Contains(nameQuery));
            }
            query = query.OrderByDescending(o => o.Order.OrderDate);

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public List<ExportOrderItem> GetExportOrderItems(int orderId,int sellerId)
        {

            var query = _context.ExportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Customer);
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            query = query.OrderByDescending(o => o.Order.OrderDate);

            return query.ToList();
        }

        public int GetTotalExportOrderItems(int orderId,string nameQuery, int sellerId)
        {
            var query = _context.ExportOrderItems.AsQueryable();
            query = query.Include(o => o.Product)
                         .Include(o => o.Order)
                         .ThenInclude(c => c.Customer);
            if (sellerId > 0)
            {
                query = query.Where(o => o.SellerId == sellerId);
            }
            if (orderId > 0)
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Order.Customer.Name.Contains(nameQuery));
            }

            return query.Count();
        }
        public void EditExportOrderItem(int orderId, int productId, ExportOrderItem order)
        {
            var existingOrder = _context.ExportOrderItems.FirstOrDefault(c => c.OrderId == orderId && c.ProductId == productId);

            if (existingOrder != null)
            {
                existingOrder.OrderId = order.OrderId;
                existingOrder.ProductId = order.ProductId;
                existingOrder.Volume = order.Volume;
                existingOrder.SellerId = order.SellerId;
                existingOrder.CreatedAt = order.CreatedAt;
                existingOrder.UpdatedAt = order.UpdatedAt;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Order not found");
            }
        }
    }
}
